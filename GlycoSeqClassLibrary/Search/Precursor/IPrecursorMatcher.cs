using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using GlycoSeqClassLibrary.Model.Chemistry.Peptide;
using GlycoSeqClassLibrary.Model.Spectrum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Search.Precursor
{
    public interface IPrecursorMatcher
    {
        List<IGlycoPeptide> Match(ISpectrum spectrum, double monoMass);
        List<IGlycoPeptide> Match(ISpectrum spectrum);
        void SetPeptides(List<IPeptide> peptides);
    }
}
