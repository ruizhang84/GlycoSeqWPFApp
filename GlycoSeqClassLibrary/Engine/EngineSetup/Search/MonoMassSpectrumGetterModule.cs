using Autofac;
using GlycoSeqClassLibrary.Algorithm;
using GlycoSeqClassLibrary.Search.Process;
using GlycoSeqClassLibrary.Search.Process.MonoMass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Engine.EngineSetup.Search
{
    public class MonoMassSpectrumGetterModule : Module
    {
        public double Tolerance { get; set; } = 5;
        public int MaxIsotop { get; set; } = 10;

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => 
            {
                IComparer<IPoint> comparer = new PPMComparer(Tolerance);
                ISearch matcherSpectrum = new BinarySearch(comparer);

                return new GeneralMonoMassSpectrumGetter(matcherSpectrum, MaxIsotop);
            }).As<IMonoMassSpectrumGetter>();
        }
    }
}
