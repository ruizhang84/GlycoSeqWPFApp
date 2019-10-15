using GlycoSeqClassLibrary.Analyze;
using GlycoSeqClassLibrary.Analyze.Result;
using GlycoSeqClassLibrary.Analyze.Score;
using GlycoSeqClassLibrary.Model.Chemistry.Glycan.TableNGlycan;
using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using GlycoSeqClassLibrary.Model.Chemistry.Peptide;
using GlycoSeqClassLibrary.Model.Spectrum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    public class TestCase11 : TestCase
    {
        public void Run()
        {

            IResults results = new GeneralResults();

            List<double> target = new List<double>() { 1, 2, 4};
            List<double> decoy = new List<double>() { -1, 0, 3 };

            for(int scan = 0; scan < Math.Max(target.Count, decoy.Count); scan++)
            {
                ISpectrum spectrum = new GeneralSpectrum(2, scan);
                List<IScore> scores = new List<IScore>();

                if (scan < target.Count)
                {
                    IScore score1 = new GeneralScore(createGlyco(), 1, 0);
                    IPeak peak1 = new GeneralPeak(1, target[scan]);
                    score1.AddScore(peak1);
                    scores.Add(new FDRScoreProxy(score1, true));
                }

                if (scan < decoy.Count)
                {
                    IScore score2 = new GeneralScore(createGlyco(), 1, 0);
                    IPeak peak2 = new GeneralPeak(1, decoy[scan]);
                    score2.AddScore(peak2);
                    scores.Add(new FDRScoreProxy(score2, false));
                }
                results.Add(spectrum, scores);
            }

            double scorecuttoff = GetScoreCutoff(results, 0, Math.Max(target.Count, decoy.Count), 0.34);

            Console.WriteLine(scorecuttoff);
            Console.Read();

        }

        public IGlycoPeptide createGlyco()
        {
            int[] structTable = new int[24];
            structTable[0] = 1;
            IPeptide peptide = new GeneralPeptide("test3", "ILGGHLDAKGSFPWQAKMVSHHNLTTGATLINEQWLLTTAK");
            IGlycoPeptide glycoPeptide = new GeneralGlycoPeptide(new ComplexNGlycan(structTable), peptide, 0);
            return glycoPeptide;
        }

        protected double GetScoreCutoff(IResults results, int start, int end, double fdr)
        {
            // init
            double cutoff = 0;

            // get score values for true and decoy results
            List<double> targets = new List<double>();
            List<double> decoys = new List<double>();
            for (int scan = start; scan <= end; scan++)
            {
                if (results.Contains(scan))
                {
                    foreach (IScore score in results.GetResult(scan))
                    {
                        if (!(score as IFDRScoreProxy).IsDecoy())
                        {
                            targets.Add(score.GetScore());
                            break;
                        }
                    }
                    foreach (IScore score in results.GetResult(scan))
                    {
                        if ((score as IFDRScoreProxy).IsDecoy())
                        {
                            decoys.Add(score.GetScore());
                            cutoff = Math.Max(cutoff, score.GetScore());
                            break;
                        }
                    }
                }
            }
            targets.Sort((a, b) => -a.CompareTo(b)); //descending
            decoys.Sort((a, b) => -a.CompareTo(b));

            // compare and compute
            int i = 0, j = 0;
            while (i < targets.Count && j < decoys.Count)
            {
                //Console.WriteLine(targets[i].ToString() + " " + decoys[j].ToString());
                if (targets[i] > decoys[j])
                {
                    double rate = j * 1.0 / (i + 1);
                    if (rate <= fdr)
                    {
                        cutoff = Math.Min(cutoff, decoys[j]);
                        Console.WriteLine(targets[i].ToString() + " " + decoys[j].ToString() + " " + rate);
                    }
                    i++;
                }
                else
                {
                    j++;
                }
            }

            if (decoys.Count * 1.0 / targets.Count < fdr)
            {
                cutoff = 0;
            }
           

            return cutoff;
        }
    }
}
