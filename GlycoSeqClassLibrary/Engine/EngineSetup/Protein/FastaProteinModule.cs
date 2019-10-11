using Autofac;
using GlycoSeqClassLibrary.Builder.Chemistry.Protein;
using GlycoSeqClassLibrary.Builder.Chemistry.Protein.Fasta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GlycoSeqClassLibrary.Engine.EngineSetup.Protein
{
    public class FastaProteinModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {            
            builder.Register(c => 
            {
                IProteinDataBuilder proteinBuilder = new GeneralFastaDataBuilder();
                return new GeneralProteinCreator(proteinBuilder);
            }).As<IProteinCreator>();
        }
    }
}
