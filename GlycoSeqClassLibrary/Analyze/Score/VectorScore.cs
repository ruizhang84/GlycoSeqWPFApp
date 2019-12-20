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
    public class VectorScore : IScore
    {
        IGlycoPeptide glycoPeptide;

        // parameters
        double alpha;
        double beta;

        // matches
        HashSet<IPeak> peakSet;
        Dictionary<MassType, List<IPeak>> matches;
        // weights
        Dictionary<MassType, double> weights;

        public VectorScore(IGlycoPeptide glycoPeptide, double alpha, double beta,
            Dictionary<MassType, double> weights)
        {
            this.glycoPeptide = glycoPeptide;

            this.alpha = alpha;
            this.beta = beta;
            this.weights = weights;
            peakSet = new HashSet<IPeak>();
            matches = new Dictionary<MassType, List<IPeak>>();
        }

        private void AddScoreWithType(IPeak peak, MassType type)
        {
            if (!matches.ContainsKey(type))
            {
                matches[type] = new List<IPeak>();
            }
            if (!peakSet.Contains(peak))
            {
                matches[type].Add(peak);
                peakSet.Add(peak);
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
            foreach (KeyValuePair<MassType, List<IPeak>> item 
                in (other as VectorScore).matches)
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
                score += GetScore(type) * weights[type];
            }
            return score;
        }

        public double GetScore(MassType type)
        {
            if (matches.ContainsKey(type))
            {
                return matches[type].Sum(x => x.GetIntensity());
            }
            return 0;
        }
    }
}
