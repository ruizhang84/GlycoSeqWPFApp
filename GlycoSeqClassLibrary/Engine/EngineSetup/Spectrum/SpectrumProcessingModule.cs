using Autofac;
using GlycoSeqClassLibrary.Search.Process;
using GlycoSeqClassLibrary.Search.Process.Normalize;
using GlycoSeqClassLibrary.Search.Process.PeakPicking;
using GlycoSeqClassLibrary.Search.Process.PeakPicking.PeakPickingDelegator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Engine.EngineSetup.Spectrum
{
    public class SpectrumProcessingModule : Module
    {      
        public double ScaleFactor { get; set; } = 1.0;

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                ISpectrumProcessing peakPicking = new GeneralPeakPickingSpectrumProcessing(c.Resolve<IPeakPickingDelegator>());
                ISpectrumProcessing normalizer = new GeneralNormalizeSpectrumProcessing(ScaleFactor);
                ISpectrumProcessingProxy spectrumProcessing = new SpectrumPRocessingProxy();
                spectrumProcessing.Add(peakPicking);
                spectrumProcessing.Add(normalizer);

                return spectrumProcessing;
            }).As<ISpectrumProcessing>();
        }
    }
}
