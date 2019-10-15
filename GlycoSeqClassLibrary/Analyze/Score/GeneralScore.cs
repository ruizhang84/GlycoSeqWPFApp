using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using GlycoSeqClassLibrary.Model.Spectrum;

namespace GlycoSeqClassLibrary.Analyze.Score
{
    public class GeneralScore : IScore
    {
        IGlycoPeptide glycoPeptide;
        HashSet<double> peakInclude;
        double score;
        double alpha;
        double beta;

        public GeneralScore(IGlycoPeptide glycoPeptide, double alpha, double beta)
        {
            this.score = 0.0;
            this.glycoPeptide = glycoPeptide;
            this.alpha = alpha;
            this.beta = beta;
            peakInclude = new HashSet<double>();
        }

        public void AddScore(IPeak peak)
        {
            double mz = peak.GetMZ();
            if (!peakInclude.Contains(mz)){
                peakInclude.Add(mz);
                score += alpha * peak.GetIntensity() + beta;
            }
        }

        public void AddScore(IScore other)
        {
            score += other.GetScore();
        }

        public IGlycoPeptide GetGlycoPeptide()
        {
            return glycoPeptide;
        }

        public double GetScore()
        {
            return score;
        }
    }
}
