using GlycoSeqClassLibrary.Analyze;
using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using GlycoSeqClassLibrary.Model.Spectrum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Search.SearchEThcD
{
    public interface ISearchEThcD
    {
       IScore Search(ISpectrum spectrum, IGlycoPeptide glycoPeptides);
       IScore SearchDecoy(ISpectrum spectrum, IGlycoPeptide glycoPeptides);
    }
}
