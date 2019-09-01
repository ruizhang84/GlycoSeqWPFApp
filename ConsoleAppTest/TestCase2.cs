using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlycoSeqClassLibrary.Builder.Chemistry.Peptide;
using GlycoSeqClassLibrary.Builder.Chemistry.Peptide.Generator;
using GlycoSeqClassLibrary.Builder.Chemistry.Peptide.Parameter;
using GlycoSeqClassLibrary.Model.Chemistry.Peptide;
using GlycoSeqClassLibrary.Model.Chemistry.Protein;

namespace ConsoleAppTest
{

    public class TestCase2 : TestCase
    {
        public void Run()
        {
            IPeptideSequencesGeneratorParameter parameter = new GeneralPeptideGeneratorParameter(Proteases.Trypsin);
            List<IPeptideSequencesGenerator> generator = new List<IPeptideSequencesGenerator>();
            generator.Add(new NGlycosylatedPeptideSequencesGenerator(parameter));
            IPeptideSequencesGeneratorParameter parameter2 = new GeneralPeptideGeneratorParameter(Proteases.GluC);
            generator.Add(new NGlycosylatedPeptideSequencesGenerator(parameter2));
            IPeptideSequencesGenerator generators = new DoubleDigestionPeptideSequencesGeneratorProxy(generator);

            List<string> sequences = generators.Generate("ILGGHLDAKGSFPWQAKMVSHHNLTTGATLINEQWLLTTAK");

            IPeptideCreator creator = new GeneralPeptideCreator(generators);
            List<IPeptide> peptidese = creator.Create(new GeneralProtein("this", "ILGGHLDAKGSFPWQAKMVSHHNLTTGATLINEQWLLTTAK"));

            foreach(IPeptide s in peptidese)
            {
                Console.WriteLine(s.GetSequence());
            }

            Console.Read();

        }
    }
}
