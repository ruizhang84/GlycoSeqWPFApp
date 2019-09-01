using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Model.Spectrum
{
    public enum TypeOfMSActivation { CID, MPD, ECD, PQD, ETD, HCD, Any, SA, PTR, NETD, NPTR };
    public interface ISpectrumMSn : ISpectrum
    {
        TypeOfMSActivation GetActivation();
        double GetParentMZ();
        int GetParentCharge();
    }
}
