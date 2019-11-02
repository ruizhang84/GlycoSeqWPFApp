using Autofac;
using GlycoSeqClassLibrary.Algorithm;
using GlycoSeqClassLibrary.Search.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Engine.EngineSetup.Search
{
    public class SpectrumFilterModule : Module
    {
        public double Tolerance { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                IComparer<IPoint> comparer = new ToleranceComparer(Tolerance);
                ISearch matcher = new BinarySearch(comparer);

                return new OxoniumSpectrumFilter(matcher);
            }).As<ISpectrumFilter>();
        }
    }
}
