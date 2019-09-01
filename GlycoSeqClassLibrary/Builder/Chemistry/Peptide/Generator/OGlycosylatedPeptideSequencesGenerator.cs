using GlycoSeqClassLibrary.Util.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Peptide.Generator
{
    public class OGlycosylatedPeptideSequencesGenerator : IPeptideSequencesGenerator
    {
        PeptideSequencesGeneratorTemplate generator;
        public OGlycosylatedPeptideSequencesGenerator(IPeptideSequencesGeneratorParameter parameter)
        {
            generator = new PeptideSequencesGeneratorTemplate(parameter);
        }

        public List<string> Generate(string sequence)
        {
            List<string> peptideList = new List<string>();
            foreach (string s in generator.Generate(sequence))
            {
                if (FindPTMPosition.ContainsOGlycanSite(s))
                {
                    peptideList.Add(s);
                }
            }
            return peptideList;
        }
    }
}
