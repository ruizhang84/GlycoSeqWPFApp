using Autofac;
using GlycoSeqClassLibrary.Builder.Chemistry.Peptide;
using GlycoSeqClassLibrary.Builder.Chemistry.Peptide.Generator;
using GlycoSeqClassLibrary.Builder.Chemistry.Peptide.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Engine.EngineSetup.Peptide
{
    public class DoubleDigestionPeptidesModule : Module
    {
        public List<string> Enzymes { get; set; }
        public int MiniLength { get; set; }
        public int MissCleavage { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => 
            {
                List<IPeptideSequencesGenerator> generatorList = new List<IPeptideSequencesGenerator>();

                foreach (string enzyme in Enzymes)
                {
                    IPeptideSequencesGeneratorParameter parameter = new GeneralPeptideGeneratorParameter();
                    parameter.SetMissCleavage(MissCleavage);
                    parameter.SetMiniLength(MiniLength);
                    switch (enzyme)
                    {
                        case "GluC":
                            parameter.SetProtease(Proteases.GluC);
                            break;
                        case "Chymotrypsin":
                            parameter.SetProtease(Proteases.Chymotrypsin);
                            break;
                        case "Pepsin":
                            parameter.SetProtease(Proteases.Pepsin);
                            break;
                        case "Trypsin":
                            parameter.SetProtease(Proteases.Trypsin);
                            break;
                        default:
                            parameter.SetProtease(Proteases.Trypsin);
                            break;
                    }
                    NGlycosylatedPeptideSequencesGenerator generator = new NGlycosylatedPeptideSequencesGenerator(parameter);
                    generatorList.Add(generator);
                }

                IPeptideSequencesGenerator peptideSequencesGenerator = new DoubleDigestionPeptideSequencesGeneratorProxy(generatorList);

                return new GeneralPeptideCreator(peptideSequencesGenerator);
            }).As<IPeptideCreator>();
        }
    }
}
