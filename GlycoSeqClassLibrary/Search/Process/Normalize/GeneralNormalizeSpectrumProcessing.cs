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

            double maxIntensity = Math.Log10(peaks.Max(x => x.GetIntensity()));
            for (int i = 0; i < peaks.Count; i++)
            {
                peaks[i].SetIntensity(Math.Log10(peaks[i].GetIntensity()) * scale / maxIntensity);
            }
            spectrum.SetPeaks(peaks);
        }
    }
}
