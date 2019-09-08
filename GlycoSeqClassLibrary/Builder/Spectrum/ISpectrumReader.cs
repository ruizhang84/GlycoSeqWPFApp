using GlycoSeqClassLibrary.Model.Spectrum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Builder.Spectrum
{
    public interface ISpectrumReader
    {
        int GetFirstScan();
        int GetLastScan();
        int GetMSnOrder(int scanNum);
        TypeOfMSActivation GetActivation(int scanNum);
        double[] GetParentMZCharge(int scanNum);
        List<IPeak> Read(int scanNum);
    }
}
