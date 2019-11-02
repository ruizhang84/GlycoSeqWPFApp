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
        double glycanWeight;
        double coreGlycanWeight;
        double peptideWeight;

        public WeightedScoreFactory(double alpha, double beta, 
            double glycanWeight, double coreGlycanWeight, double peptideWeight)
        {
            this.alpha = alpha * 100;
            this.beta = beta;
            this.glycanWeight = glycanWeight;
            this.coreGlycanWeight = coreGlycanWeight;
            this.peptideWeight = peptideWeight;
        }

        public IScore CreateScore(IGlycoPeptide glycoPeptide)
        {
            return new WeightedScore(glycoPeptide, alpha, beta, glycanWeight, coreGlycanWeight, peptideWeight);
        }
    }
}
