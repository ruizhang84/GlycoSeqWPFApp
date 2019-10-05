using GlycoSeqClassLibrary.Model.Spectrum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Search.Process
{
    public interface ISpectrumProcessing
    {
        void Process(ISpectrum spectrum);
    }
}
