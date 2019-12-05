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

        public WeightedScore(IGlycoPeptide glycoPeptide, 
            double alpha, double beta, Dictionary<MassType, double> weights)
        {
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
            foreach (KeyValuePair<MassType, List<double>> item in (other as WeightedScore).matches)
            {
                if (matches.ContainsKey(item.Key))
                {
                    matches[item.Key].AddRange(item.Value);
                }
                else
                {
                    matches[item.Key] = item.Value;
                }
            }
        }

        public IGlycoPeptide GetGlycoPeptide()
        {
            return glycoPeptide;
        }

        public double GetScore()
        {
            double score = 0;
            foreach (MassType type in matches.Keys)
            {
                score += GetScore(type);
            }
            return score;
        }

        public double GetScore(MassType type)
        {
            if (matches.ContainsKey(type))
            {
                double scale = Math.Pow(matches[type].Count / theory[type], weights[type]);
                return matches[type].Sum() * scale;
            }
            return 0;
        }
    }
}
