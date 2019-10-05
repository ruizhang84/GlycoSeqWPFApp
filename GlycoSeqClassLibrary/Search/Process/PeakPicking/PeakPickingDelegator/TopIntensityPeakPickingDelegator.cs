using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlycoSeqClassLibrary.Model.Spectrum;

namespace GlycoSeqClassLibrary.Search.Process.PeakPicking.PeakPickingDelegator
{
    public class TopIntensityPeakPickingDelegator : IPeakPickingDelegator
    {
        int maxPeaks;
        public TopIntensityPeakPickingDelegator(int maxPeaks)
        {
            this.maxPeaks = maxPeaks;
        }

        public List<IPeak> Picking(List<IPeak> peaks)
        {
            return peaks
                .OrderByDescending(pk => pk.GetIntensity())
                .Take(maxPeaks)
                .OrderBy(pk => pk.GetMZ()).ToList();
        }
    }
}
