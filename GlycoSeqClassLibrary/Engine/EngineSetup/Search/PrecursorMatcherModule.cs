using Autofac;
using GlycoSeqClassLibrary.Algorithm;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide;
using GlycoSeqClassLibrary.Search.Precursor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Engine.EngineSetup.Search
{
    public class PrecursorMatcherModule : Module
    {
        public double Tolerance { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => 
            {
                IComparer<IPoint> comparer = new PPMComparer(Tolerance);
                ISearch matcher = new BinarySearch(comparer);

                return new GeneralPrecursorMatcher(matcher, c.Resolve<IGlycoPeptideCreator>());
            }).As<IPrecursorMatcher>();

        }
    }
}
