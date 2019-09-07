using GlycoSeqClassLibrary.Builder.Chemistry.Glycan.Mass;
using GlycoSeqClassLibrary.Model.Chemistry.Glycan;
using GlycoSeqClassLibrary.Model.Chemistry.Peptide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide
{
    public interface IGlycoPeptideProxyGenerator
    {
        IGlycoPeptideProxy Generate(IGlycan glycan, IPeptide peptide, int modifySite);
    }
}
