using GlycoSeqClassLibrary.Model.Spectrum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Search.Process
{
    public interface IMonoMassSpectrumGetter
    {
        double GetMonoMass(ISpectrum MSnSpectrum);
    }
}
