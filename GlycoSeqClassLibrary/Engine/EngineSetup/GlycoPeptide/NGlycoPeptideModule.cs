using Autofac;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide.Mass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Engine.EngineSetup.GlycoPeptide
{
    public class NGlycoPeptideModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            
            builder.Register(c => 
            {
                IGlycoPeptideProxyGenerator glycoPeptideProxyGenerator = new GeneralTableNGlycoPeptideMassProxyGenerator();
                return new GeneralNGlycoPeptideSingleSiteCreator(glycoPeptideProxyGenerator);
            }).As<IGlycoPeptideCreator>();

        }
    }
}
