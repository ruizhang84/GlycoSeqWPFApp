using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Algorithm
{
    public class ToleranceComparer : Comparer<IPoint>
    {
        public double Tolerance { get; set; }

        public ToleranceComparer(double tol)
        {
            Tolerance = tol;
        }

        public override int Compare(IPoint x, IPoint y)
        {
            if (Math.Abs(x.GetValue() - y.GetValue()) < Tolerance) return 0;
            else if (x.GetValue() < y.GetValue()) return -1;
            return 1;
        }
    }
}
