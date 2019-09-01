using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlycoSeqClassLibrary.Model.Chemistry.Peptide;
using GlycoSeqClassLibrary.Model.Chemistry.Protein;
using GlycoSeqClassLibrary.Util.Model;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Peptide.Generator
{
    public class NGlycosylatedPeptideSequencesGenerator : IPeptideSequencesGenerator
    {
        PeptideSequencesGeneratorTemplate generator;
        public NGlycosylatedPeptideSequencesGenerator(IPeptideSequencesGeneratorParameter parameter)
        {
            generator = new PeptideSequencesGeneratorTemplate(parameter);
        }

        public List<string> Generate(string sequence)
        {
            List<string> peptideList = new List<string>();
            foreach (string s in generator.Generate(sequence))
            {
                if (FindPTMPosition.ContainsNGlycanSite(s))
                {
                    peptideList.Add(s);
                }
            }
            return peptideList;
        }
    }
}
