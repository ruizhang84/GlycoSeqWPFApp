using Autofac;
using GlycoSeqClassLibrary.Algorithm;
using GlycoSeqClassLibrary.Analyze;
using GlycoSeqClassLibrary.Analyze.Score;
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
        public double peptideWeight { get; set; } = 0.0;

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => 
            {
                IComparer<IPoint> comparer = new ToleranceComparer(Tolerance);
                ISearch matcherPeaks = new BinarySearch(comparer);
                IScoreFactory scoreFactory = new WeightedScoreFactory(alpha, beta, glycanWeight, peptideWeight);
                IGlycoPeptidePointsCreator glycoPeptidePointsCreator = new GeneralGlycoPeptideMassProxyPointsCreator();

                return new GeneralSearchEThcD(matcherPeaks, scoreFactory, glycoPeptidePointsCreator);
            }).As<ISearchEThcD>();
        }
    }
}
