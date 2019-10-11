using Autofac;
using GlycoSeqClassLibrary.Analyze;
using GlycoSeqClassLibrary.Analyze.Reporter;
using GlycoSeqClassLibrary.Analyze.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Engine.EngineSetup.Report
{
    public class CSVReportModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new GeneralResults()).As<IResults>();
            builder.Register(c => new CSVReportProducer()).As<IReportProducer>();
        }
    }
}
