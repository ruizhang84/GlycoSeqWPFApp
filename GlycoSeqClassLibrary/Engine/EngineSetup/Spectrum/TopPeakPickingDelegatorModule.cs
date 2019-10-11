using Autofac;
using GlycoSeqClassLibrary.Search.Process.PeakPicking;
using GlycoSeqClassLibrary.Search.Process.PeakPicking.PeakPickingDelegator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Engine.EngineSetup.Spectrum
{
    public class TopPeakPickingDelegatorModule : Module
    {
        public int MaxPeaks { get; set; } = 100;

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new TopIntensityPeakPickingDelegator(MaxPeaks)).As<IPeakPickingDelegator>();
        }
    }
}
