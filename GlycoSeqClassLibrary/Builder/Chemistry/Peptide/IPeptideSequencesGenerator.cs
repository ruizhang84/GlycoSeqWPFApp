using GlycoSeqClassLibrary.Model.Chemistry.Protein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Peptide
{
    public interface IPeptideSequencesGenerator
    {
        List<string> Generate(string sequence);
    }
}
