using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Peptide.Generator
{
    public class DoubleDigestionPeptideSequencesGeneratorProxy : IPeptideSequencesGenerator
    {
        List<IPeptideSequencesGenerator> generators;

        public DoubleDigestionPeptideSequencesGeneratorProxy(List<IPeptideSequencesGenerator> generatorList)
        {
            generators = new List<IPeptideSequencesGenerator>();
            for(int i = 0; i < generatorList.Count; i++)
            {
                generators.Add(generatorList[i]);
            }
        }

        public List<string> Generate(string sequence)
        {
            HashSet<string> sequenceSet = new HashSet<string>();
            for (int i = 0; i < generators.Count; i++)
            {
                if (i == 0)
                {
                    sequenceSet.UnionWith(generators[0].Generate(sequence));
                }
                else
                {
                    foreach(string seq in sequenceSet.ToList())
                    {
                        sequenceSet.UnionWith(generators[i].Generate(seq));
                    }
                }
            }

            return sequenceSet.ToList();
        }
    }
}
