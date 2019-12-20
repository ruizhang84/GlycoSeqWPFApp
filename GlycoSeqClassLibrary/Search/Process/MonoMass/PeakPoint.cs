using GlycoSeqClassLibrary.Algorithm;
using GlycoSeqClassLibrary.Model.Spectrum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Search.Process.MonoMass
{
    public class PeakPoint : IPoint
    {
        public double MZ { get; set; }
        public double Intensity { get; set; }

        public PeakPoint(IPeak peak)
        {
            MZ = peak.GetMZ();
            Intensity = peak.GetIntensity();
        }

        public int CompareTo(IPoint other)
        {
            return GetValue().CompareTo(other.GetValue());
        }

        public double GetValue()
        {
            return MZ;
        }
    }
}
