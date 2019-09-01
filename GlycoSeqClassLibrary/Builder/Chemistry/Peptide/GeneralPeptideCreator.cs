using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlycoSeqClassLibrary.Model.Chemistry.Peptide;
using GlycoSeqClassLibrary.Model.Chemistry.Protein;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Peptide
{
    public class GeneralPeptideCreator : IPeptideCreator
    {
        IPeptideSequencesGenerator generator;
        public GeneralPeptideCreator(IPeptideSequencesGenerator g)
        {
            generator = g;
        }

        public List<IPeptide> Create(IProtein protein)
        {
            List<IPeptide> peptideList = new List<IPeptide>();
            List<string> sequences = generator.Generate(protein.GetSequence());
            foreach(string seq in sequences)
            {
                GeneralPeptide peptide = new GeneralPeptide(protein.GetID(), seq);
                peptideList.Add(peptide);
            }
            protein.SetPeptides(peptideList);
            return peptideList;
        }
    }
}
