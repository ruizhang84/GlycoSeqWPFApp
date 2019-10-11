using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Search.Process
{
    public interface ISpectrumProcessingProxy : ISpectrumProcessing
    {
        void Add(ISpectrumProcessing spectrumProcessing);
        void Clear();
    }
}
