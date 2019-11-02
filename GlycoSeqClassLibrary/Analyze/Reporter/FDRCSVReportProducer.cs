using GlycoSeqClassLibrary.Analyze.Score;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Analyze.Reporter
{
    public class FDRCSVReportProducer : CSVReportProducer
    {
        double fdr;

        public FDRCSVReportProducer(double rate) : base()
        {
            fdr = rate;
        }

        protected double GetScoreCutoff(IResults results, int start, int end)
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


        public override void Report(IResults results, int start, int end)
        {
            scoreCutoff = GetScoreCutoff(results, start, end);
            if (InitSucess(scoreCutoff))
            {
                for (int scanNum = start; scanNum <= end; scanNum++)
                {
                    if (results.Contains(scanNum))
                    {
                        //List<IScore> scores = results.GetResult(scanNum);
                        List<IScore> scores = results.GetResult(scanNum).Where(score => !(score as IFDRScoreProxy).IsDecoy()).ToList();
                        ReportLines(results.GetSpectrum(scanNum), scores, scoreCutoff);
                    }
                }
                Exit();
            }
        }
    }
}
