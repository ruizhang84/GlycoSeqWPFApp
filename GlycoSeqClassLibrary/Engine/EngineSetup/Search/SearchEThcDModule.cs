using Autofac;
using GlycoSeqClassLibrary.Algorithm;
using GlycoSeqClassLibrary.Analyze;
using GlycoSeqClassLibrary.Analyze.Score;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide.Mass;
using GlycoSeqClassLibrary.Search.SearchEThcD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Engine.EngineSetup.Search
{
    public class SearchEThcDModule : Module
    {
        public double Tolerance { get; set; }
        public double alpha { get; set; } = 1.0;
        public double beta { get; set; } = 0.0;
        public double glycanWeight { get; set; } = 1.0;
        public double coreGlycanWeight { get; set; } = 1.0;
        public double branchGlycanWeight { get; set; } = 1.0;
        public double peptideWeight { get; set; } = 0.0;

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => 
            {
                IComparer<IPoint> comparer = new ToleranceComparer(Tolerance);
                ISearch matcherPeaks = new BinarySearch(comparer);

                Dictionary<MassType, double> weights = new Dictionary<MassType, double>();
                weights.Add(MassType.Core, coreGlycanWeight);
                weights.Add(MassType.Branch, branchGlycanWeight);
                weights.Add(MassType.Glycan, glycanWeight);
                weights.Add(MassType.Peptide, peptideWeight);
                IScoreFactory scoreFactory = new WeightedScoreFactory(alpha, beta, weights);
                IGlycoPeptidePointsCreator glycoPeptidePointsCreator = new GeneralGlycoPeptideMassProxyPointsCreator();

                return new GeneralSearchEThcD(matcherPeaks, scoreFactory, glycoPeptidePointsCreator);
            }).As<ISearchEThcD>();
        }
    }
}
