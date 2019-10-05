using GlycoSeqClassLibrary.Model.Spectrum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Search.Process
{
    interface IMonoMassSpectrumGetter
    {
        double GetMonoMass(ISpectrum MSnSpectrum);
        void SetMonoMassSpectrum(ISpectrum MS1Spectrum);
    }
}
