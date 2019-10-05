using GlycoSeqClassLibrary.Model.Spectrum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Search.Process.PeakPicking
{
    public class GeneralPeakPickingSpectrumProcessing : ISpectrumProcessing
    {
        IPeakPickingDelegator peakPicker;
        public GeneralPeakPickingSpectrumProcessing(IPeakPickingDelegator peakPicker)
        {
            this.peakPicker = peakPicker;
        }

        public void Process(ISpectrum spectrum)
        {
            List<IPeak> peaks = peakPicker.Picking(spectrum.GetPeaks());
            spectrum.SetPeaks(peaks);
        }
    }
}
