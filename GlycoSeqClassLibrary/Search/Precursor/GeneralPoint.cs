using GlycoSeqClassLibrary.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Search.Precursor
{
    public class GeneralPoint : IPoint
    {
        double mass;

        public GeneralPoint(double mass)
        {
            this.mass = mass;
        }

        public int CompareTo(IPoint other)
        {
            return GetValue().CompareTo(other.GetValue());
        }

        public double GetValue()
        {
            return mass;
        }
    }
}
