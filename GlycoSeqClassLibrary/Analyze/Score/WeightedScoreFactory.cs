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
            this.alpha = alpha * 100;
            this.beta = beta;
            this.glycanWeight = glycanWeight;
            this.peptideWeight = peptideWeight;
        }

        public IScore CreateScore(IGlycoPeptide glycoPeptide, object parameter)
        {

            MassType type = (MassType) parameter;
            double normalizedBeta = beta; //* 1.0 / (glycoPeptide as IGlycoPeptideMassProxy).GetMass(type).Count;
            switch (type)
            {
                case MassType.All:
                    return new WeightedScore(glycoPeptide, alpha, normalizedBeta, 1.0);
                case MassType.Glycan:
                    return new WeightedScore(glycoPeptide, alpha, normalizedBeta, glycanWeight);
                case MassType.Peptide:
                    return new WeightedScore(glycoPeptide, alpha, normalizedBeta, peptideWeight);
                case MassType.ReverseGlycan:
                    return new WeightedScore(glycoPeptide, alpha, normalizedBeta, glycanWeight);
                case MassType.ReversePeptide:
                    return new WeightedScore(glycoPeptide, alpha, normalizedBeta, peptideWeight);
                default:
                    return new WeightedScore(glycoPeptide, alpha, normalizedBeta, 0.0);
            }
        }
    }
}
