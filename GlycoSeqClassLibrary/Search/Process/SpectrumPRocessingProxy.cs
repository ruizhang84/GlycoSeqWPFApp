using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlycoSeqClassLibrary.Model.Spectrum;

namespace GlycoSeqClassLibrary.Search.Process
{
    public class SpectrumPRocessingProxy : ISpectrumProcessingProxy
    {
        List<ISpectrumProcessing> spectrumProcessores;

        public SpectrumPRocessingProxy()
        {
            spectrumProcessores = new List<ISpectrumProcessing>();
        }

        public void Add(ISpectrumProcessing spectrumProcessing)
        {
            spectrumProcessores.Add(spectrumProcessing);
        }

        public void Clear()
        {
            spectrumProcessores.Clear();
        }

        public void Process(ISpectrum spectrum)
        {
            foreach(ISpectrumProcessing spectrumProcessor in spectrumProcessores)
            {
                spectrumProcessor.Process(spectrum);
            }
            
        }
    }
}
