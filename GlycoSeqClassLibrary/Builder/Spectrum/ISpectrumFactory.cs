using GlycoSeqClassLibrary.Model.Spectrum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Builder.Spectrum
{
    public interface ISpectrumFactory
    {
        int GetFirstScan();
        int GetLastScan();
        int GetMSnOrder(int scanNum);
        TypeOfMSActivation GetActivation(int scanNum);
        ISpectrum GetSpectrum(int scanNum);
    }
}
