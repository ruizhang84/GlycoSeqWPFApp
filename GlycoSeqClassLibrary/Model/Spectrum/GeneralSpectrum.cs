using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Model.Spectrum
{
    public class GeneralSpectrum : ISpectrum
    {
        protected int msOrder;
        protected int scanNum;
        protected List<IPeak> peaks;

        public GeneralSpectrum(int msOrder, int scanNum)
        {
            this.msOrder = msOrder;
            this.scanNum = scanNum;
            peaks = new List<IPeak>();
        }
        public void Add(IPeak peak)
        {
            peaks.Add(peak);
        }
        public void Clear()
        {
            peaks.Clear();
        }

        public int GetMSnOrder()
        {
            return msOrder;
        }

        public List<IPeak> GetPeaks()
        {
            return peaks;
        }

        public int GetScanNum()
        {
            return scanNum;
        }

        public void SetPeaks(List<IPeak> peaks)
        {
            this.peaks = peaks;
        }
    }
}
