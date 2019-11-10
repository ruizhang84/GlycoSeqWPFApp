using GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide.Mass;
using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Analyze.Score
{
    public class WeightedScoreFactory : IScoreFactory
    {
        double alpha;
        double beta;
        Dictionary<MassType, double> weights;

        public WeightedScoreFactory(double alpha, double beta, Dictionary<MassType, double> weights)
        {
            this.alpha = alpha * 100;
            this.beta = beta;
            this.weights = weights;
        }

        public IScore CreateScore(IGlycoPeptide glycoPeptide)
        {
            return new WeightedScore(glycoPeptide, alpha, beta, weights);
        }
    }
}
