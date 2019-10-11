using Autofac;
using GlycoSeqClassLibrary.Builder.Spectrum;
using GlycoSeqClassLibrary.Builder.Spectrum.ThermoRaw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Engine.EngineSetup.Spectrum
{
    public class ThermoRawSpectrumModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new ThermoRawSpectrumReader()).As<ISpectrumReader>();
            builder.Register(c => new GeneralSpectrumFactory(c.Resolve<ISpectrumReader>())).As<ISpectrumFactory>();

        }
    }
}
