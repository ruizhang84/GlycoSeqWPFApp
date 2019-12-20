using GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide.Mass;
using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Analyze.Score
{
    public class VectorScoreFactory : IScoreFactory
    {
        double alpha;
        double beta;
        Dictionary<MassType, double> weights;

        public VectorScoreFactory(double alpha, double beta, Dictionary<MassType, double> weights)
        {
            this.alpha = alpha * 100;
            this.beta = beta;
            this.weights = weights;
        }

        public IScore CreateScore(IGlycoPeptide glycoPeptide)
        {
            return new VectorScore(glycoPeptide, alpha, beta, weights);
        }
    }
}
