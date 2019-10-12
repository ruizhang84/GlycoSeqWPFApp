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
        double peptideWeight;

        public WeightedScoreFactory(double alpha, double beta, double glycanWeight, double peptideWeight)
        {
            this.alpha = alpha;
            this.beta = beta;
            this.glycanWeight = glycanWeight;
            this.peptideWeight = peptideWeight;
        }

        public IScore CreateScore(IGlycoPeptide glycoPeptide, object parameter)
        {

            MassType type = (MassType) parameter;
            switch (type)
            {
                case MassType.All:
                    return new WeightedScore(glycoPeptide, alpha, beta, 1.0);
                case MassType.Glycan:
                    return new WeightedScore(glycoPeptide, alpha, beta, glycanWeight);
                case MassType.Peptide:
                    return new WeightedScore(glycoPeptide, alpha, beta, peptideWeight);
                default:
                    return new WeightedScore(glycoPeptide, alpha, beta, 0.0);
            }
        }
    }
}
