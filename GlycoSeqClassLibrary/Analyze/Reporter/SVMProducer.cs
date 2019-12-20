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
    public class SVMProducer : IReportProducer
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
                writer.Write("predict, ");
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

        public void Training(IResults results, int start, int end)
        {
            for (int scanNum = start; scanNum <= end; scanNum++)
            {
                if (results.Contains(scanNum))
                {
                    List<IScore> scores = results.GetResult(scanNum);
                    foreach(IScore score in scores)
                    {
                        double y = (score as FDRScoreProxy).IsDecoy() ? 0 : 1;
                        List<SVMNode> X = new List<SVMNode>();
                        // store score value in X
                        int idx = 0;
                        foreach(MassType type in types)
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
            model = problem.Train(parameter);
        }

        public Tuple<List<double[]>, double[]> Predicting(List<IScore> scores)
         {
            List<double[]> estimationList = new List<double[]>();

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
            
            double [] target = testingProblem.PredictProbability(model, out estimationList);
            testingProblem.X.Clear();
            return Tuple.Create(estimationList, target);
        }

        public void ReportLines(ISpectrum spectrum, List<IScore> scores)
        {
            HashSet<string> seen = new HashSet<string>();


            Tuple<List<double[]>, double[]> predict = Predicting(scores);
            int idx = 0;
            foreach (IScore score in scores)
            {

                if (0 >= 0)
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

                    writer.Write(spectrum.GetScanNum().ToString() + ", ");
                    writer.Write(seq + ", ");
                    writer.Write(structure + ", ");
                    writer.Write(score.GetScore(MassType.Core).ToString() + ", ");
                    writer.Write(score.GetScore(MassType.Branch).ToString() + ", ");
                    writer.Write(score.GetScore(MassType.Glycan).ToString() + ", ");
                    writer.Write(score.GetScore(MassType.Peptide).ToString() + ", ");
                    writer.Write((spectrum as ISpectrumMSn).GetParentMZ().ToString() + ", ");
                    writer.Write((spectrum as ISpectrumMSn).GetParentCharge().ToString() + ", ");
                    writer.Write(string.Join(".", predict.Item1[idx]) + ", ");
                    writer.Write(predict.Item2[idx].ToString() + ", ");
                    writer.WriteLine();
                }
                idx++;
            }
        }

        public void Report(IResults results, int start, int end)
        {
            if (Init())
            {
                // training svm
                Training(results, start, end);

                // predict and report
                for (int scanNum = start; scanNum <= end; scanNum++)
                {
                    if (results.Contains(scanNum))
                    {
                        List<IScore> scores = results.GetResult(scanNum);
                        //List<IScore> scores = results.GetResult(scanNum).
                        //    Where(score => !(score as IFDRScoreProxy).IsDecoy()).ToList();

                        ReportLines(results.GetSpectrum(scanNum), scores);
                    }
                }
                Exit();
            } 
        }

        public void SetOutputLocation(string location)
        {
            output = location;
        }
    }
}
