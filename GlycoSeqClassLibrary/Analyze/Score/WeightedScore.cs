using GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide.Mass;
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

        // parameters
        double extraScore;
        double alpha;
        double beta;

        // peaks
        HashSet<double> peakInclude;
        // theoritical
        Dictionary<MassType, int> theory;
        // matches
        Dictionary<MassType, List<double>> matches;
        // weights
        Dictionary<MassType, double> weights;

        public WeightedScore(IGlycoPeptide glycoPeptide, double alpha, double beta, Dictionary<MassType, double> weights)
        {
            this.extraScore = 0.0;
            this.glycoPeptide = glycoPeptide;

            this.alpha = alpha;
            this.beta = beta;

            peakInclude = new HashSet<double>();
            theory = new Dictionary<MassType, int>();
            matches = new Dictionary<MassType, List<double>>();
            this.weights = weights;

            foreach(MassType type in weights.Keys)
            {
                theory[type] = (glycoPeptide as IGlycoPeptideMassProxy).GetMass(type).Count;
            }
        }

        private double ComputeScore()
        {
            double score = 0;
            double scale = 1.0;
            foreach(MassType type in matches.Keys)
            {
                scale *= Math.Pow(matches[type].Count / theory[type], weights[type]);
            }
            foreach (MassType type in matches.Keys)
            {
                score += matches[type].Sum() * scale;
            }
            return score;
        }

        private void AddScoreWithType(IPeak peak, MassType type)
        {
            if (!matches.ContainsKey(type))
            {
                matches[type] = new List<double>();
            }

            double mz = peak.GetMZ();
            if (!peakInclude.Contains(mz))
            {
                peakInclude.Add(mz);
                matches[type].Add(alpha * peak.GetIntensity() + beta);
            }
        }

        public void AddCoreScore(IPeak peak)
        {
            AddScoreWithType(peak, MassType.Core);
        }

        public void AddBranchScore(IPeak peak)
        {
            AddScoreWithType(peak, MassType.Branch);
        }

        public void AddPeptideScore(IPeak peak)
        {
            AddScoreWithType(peak, MassType.Peptide);
        }

        public void AddScore(IPeak peak)
        {
            AddScoreWithType(peak, MassType.Glycan);
        }

        public void AddScore(IScore other)
        {
            extraScore += other.GetScore() ;
        }

        public IGlycoPeptide GetGlycoPeptide()
        {
            return glycoPeptide;
        }

        public double GetScore()
        {
            return ComputeScore() + extraScore;
        }
    }
}
