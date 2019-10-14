using GlycoSeqClassLibrary.Model.Spectrum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Analyze
{
    public interface IResults
    {
        bool Contains(int scaNum);
        List<int> GetScans();
        List<IScore> GetResult(int scanNum);
        ISpectrum GetSpectrum(int scanNum);
        void Add(ISpectrum spectrum, List<IScore> score);
        void Merge(IResults results);
        void Clear();
    }
}
