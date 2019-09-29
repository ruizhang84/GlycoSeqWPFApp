using GlycoSeqClassLibrary.Util.CalcMass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Algorithm
{
    public class PPMComparer :Comparer<IPoint>
    {
        public double PPM { get; set; }

        public PPMComparer(double tol)
        {
            PPM = tol;
        }

        public override int Compare(IPoint x, IPoint y)
        {
            if (SpectrumCalcMass.Instance.ComputePPM(x.GetValue(), y.GetValue()) < PPM) return 0;
            else if (x.GetValue() < y.GetValue()) return -1;
            return 1;
        }
    }
}
