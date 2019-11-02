using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlycoSeqClassLibrary.Model.Spectrum;

namespace GlycoSeqClassLibrary.Search.Process.Normalize
{
    public class GeneralNormalizeSpectrumProcessing : ISpectrumProcessing
    {
        double scale;

        public GeneralNormalizeSpectrumProcessing(double scale)
        {
            this.scale = scale;
        }

        public void Process(ISpectrum spectrum)
        {
            List<IPeak> peaks = spectrum.GetPeaks();

            double sumIntensity = peaks.Max(x => x.GetIntensity());
            for (int i = 0; i < peaks.Count; i++)
            {
                peaks[i].SetIntensity(peaks[i].GetIntensity() * scale / sumIntensity);
            }
            spectrum.SetPeaks(peaks);
        }
    }
}
