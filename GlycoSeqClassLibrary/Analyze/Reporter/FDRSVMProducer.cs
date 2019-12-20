using GlycoSeqClassLibrary.Analyze.Score;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide.Mass;
using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using GlycoSeqClassLibrary.Model.Spectrum;
using LibSVMsharp;
using LibSVMsharp.Extensions;
using LibSVMsharp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Analyze.Reporter
{

    public class FDRSVMProducer : IReportProducer
    {
        protected SVMProblem problem = new SVMProblem();
        protected SVMProblem testingProblem = new SVMProblem();
        protected List<MassType> types = new List<MassType>()
            {MassType.Core, MassType.Branch, MassType.Glycan, MassType.Peptide };
        protected SVMModel model;
        //protected IntPtr ptr_model;

        protected StreamWriter writer;
        protected FileStream ostrm;
        protected string output;

        protected double fdr;

        public FDRSVMProducer(double rate)
        {
            fdr = rate;
        }

        public bool Init()
        {
            try
            {
                ostrm = new FileStream(output, FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(ostrm);
                writer.Write("scan, ");
                writer.Write("peptide, ");
                writer.Write("glycan, ");
                writer.Write("Core score, ");
                writer.Write("Branch score, ");
                writer.Write("Glycan score, ");
                writer.Write("Peptide score, ");
                writer.Write("mz, ");
                writer.Write("charge, ");
                writer.Write("score, ");
                writer.Write("cutoff score, ");
                writer.WriteLine();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open file for writing log!");
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public void Exit()
        {
            writer.Close();
            ostrm.Close();
            //SVMModel.Free(ptr_model);
        }

        public void ReportLines(List<IProbScoreProxy> scores, double cutoff)
        {
            HashSet<string> seen = new HashSet<string>();


            int idx = 0;
            foreach (IProbScoreProxy score in scores)
            {

                if (score.GetProbability() > cutoff)
                {
                    // get glycan
                    IGlycoPeptide glycoPeptide = score.GetGlycoPeptide();

                    // remove redudant
                    string structure = string.Join("_", glycoPeptide.GetGlycan().GetStructure());
                    string seq = glycoPeptide.GetPeptide().GetSequence();
                    if (!seen.Contains(structure + seq))
                    {
                        seen.Add(structure + seq);
                    }
                    else
                    {
                        continue;
                    }

                    writer.Write(score.GetSpectrum().GetScanNum().ToString() + ", ");
                    writer.Write(seq + ", ");
                    writer.Write(structure + ", ");
                    writer.Write(score.GetScore(MassType.Core).ToString() + ", ");
                    writer.Write(score.GetScore(MassType.Branch).ToString() + ", ");
                    writer.Write(score.GetScore(MassType.Glycan).ToString() + ", ");
                    writer.Write(score.GetScore(MassType.Peptide).ToString() + ", ");
                    writer.Write((score.GetSpectrum() as ISpectrumMSn).GetParentMZ().ToString() + ", ");
                    writer.Write((score.GetSpectrum() as ISpectrumMSn).GetParentCharge().ToString() + ", ");
                    writer.Write(score.GetProbability().ToString() + ", ");
                    writer.Write(cutoff + ", ");
                    writer.WriteLine();
                }
                idx++;
            }
        }

        public void Training(IResults results, int start, int end)
        {
            for (int scanNum = start; scanNum <= end; scanNum++)
            {
                if (results.Contains(scanNum))
                {
                    List<IScore> scores = results.GetResult(scanNum);
                    foreach (IScore score in scores)
                    {
                        double y = (score as FDRScoreProxy).IsDecoy() ? 0 : 1;
                        List<SVMNode> X = new List<SVMNode>();
                        // store score value in X
                        int idx = 0;
                        foreach (MassType type in types)
                        {
                            SVMNode node = new SVMNode();
                            node.Index = idx;
                            node.Value = score.GetScore(type);
                            X.Add(node);
                            idx++;
                        }
                        problem.Add(X.ToArray(), y);
                    }
                }
            }

            // training
            SVMParameter parameter = new SVMParameter();
            parameter.Probability = true;
            parameter.Kernel = SVMKernelType.LINEAR;
            model = problem.Train(parameter);
        }

        public List<IProbScoreProxy> Predicting(IResults results, int scanNum)
        {
            List<IScore> scores = results.GetResult(scanNum);
            ISpectrum spectrum = results.GetSpectrum(scanNum);
            List<double[]> estimationList = new List<double[]>();
            List<IProbScoreProxy> probabilities = new List<IProbScoreProxy>();
            foreach (IScore score in scores)
            {
                List<SVMNode> X = new List<SVMNode>();
                // store score value in X
                int idx = 0;
                foreach (MassType type in types)
                {
                    SVMNode node = new SVMNode();
                    node.Index = idx;
                    node.Value = score.GetScore(type);
                    X.Add(node);
                    idx++;
                }
                testingProblem.X.Add(X.ToArray());  
            }
            // prediction
            double[] target = testingProblem.PredictProbability(model, out estimationList);
            testingProblem.X.Clear();
            // create new scores
            for(int i = 0; i < scores.Count; i++)
            {
                probabilities.Add(new ProbScoreProxy(scores[i], spectrum, estimationList[i][1])); 
            }
            return probabilities;
        }

        protected double GetScoreCutoff(List<IProbScoreProxy> probabilities, int start, int end)
        {
            // init
            double cutoff = 0;

            // get score values for true and decoy results
            List<double> targets = new List<double>();
            List<double> decoys = new List<double>();
            foreach(IProbScoreProxy probability in probabilities)
            {
                if (!probability.IsDecoy())
                {
                    targets.Add(probability.GetProbability());
                }
                else
                {
                    decoys.Add(probability.GetProbability());
                }
            }
            targets.Sort((a, b) => -a.CompareTo(b)); //descending
            decoys.Sort((a, b) => -a.CompareTo(b));

            // compare and compute
            if (decoys.Count * 1.0 / (decoys.Count + targets.Count) < fdr)   //trivial case
            {
                return 0;
            }

            int i = 0, j = 0;
            while (i < targets.Count && j < decoys.Count)
            {

                if (targets[i] > decoys[j])
                {
                    double rate = j * 1.0 / (i + j + 1);
                    if (rate <= fdr)
                    {
                        cutoff = Math.Min(cutoff, decoys[j]);
                    }
                    i++;
                }
                else
                {
                    j++;
                }
            }

            return cutoff;
        }

        public void Report(IResults results, int start, int end)
        {
            if (Init())
            {
                // training svm
                Training(results, start, end);

                // predicting probability
                List<IProbScoreProxy> probabilities = new List<IProbScoreProxy>();
                for (int scanNum = start; scanNum <= end; scanNum++)
                {
                    if (results.Contains(scanNum))
                    {
                        probabilities.AddRange(Predicting(results, scanNum));
                    }
                }
                double scoreCutoff = GetScoreCutoff(probabilities, start, end);

                // report
                List<IProbScoreProxy> scores = probabilities.
                    Where(score => !(score as IFDRScoreProxy).IsDecoy()).ToList();
                ReportLines(scores, scoreCutoff);

                Exit();
            }
        }

        public void SetOutputLocation(string location)
        {
            output = location;
        }
    }
}
