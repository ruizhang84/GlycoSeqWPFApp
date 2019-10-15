using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using GlycoSeqClassLibrary.Model.Spectrum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Analyze.Score
{
    public class WeightedScore : IScore
    {
        IGlycoPeptide glycoPeptide;
        HashSet<double> peakInclude;
        double score;
        double alpha;
        double beta;
        double weight;

        public WeightedScore(IGlycoPeptide glycoPeptide, double alpha, double beta, double weight)
        {
            this.score = 0.0;
            this.glycoPeptide = glycoPeptide;
            this.alpha = alpha;
            this.beta = beta;
            this.weight = weight;
            peakInclude = new HashSet<double>();
        }

        public void AddScore(IPeak peak)
        {
            double mz = peak.GetMZ();
            if (!peakInclude.Contains(mz))
            {
                peakInclude.Add(mz);
                score += (alpha * peak.GetIntensity() + beta) * weight;
            }
        }

        public void AddScore(IScore other)
        {
           score += other.GetScore() ;
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
