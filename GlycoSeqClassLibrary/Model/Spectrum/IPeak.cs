using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Model.Spectrum
{
    public interface IPeak : IComparable<IPeak>
    {
        double GetMZ();
        void SetMZ(double mz);
        double GetIntensity();
        void SetIntensity(double intensity);
    }
}
