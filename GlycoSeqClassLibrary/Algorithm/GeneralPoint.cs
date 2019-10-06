using GlycoSeqClassLibrary.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Algorithm
{
    public class GeneralPoint : IPoint
    {
        protected double value;

        public GeneralPoint(double value)
        {
            this.value = value;
        }

        public int CompareTo(IPoint other)
        {
            return GetValue().CompareTo(other.GetValue());
        }

        public double GetValue()
        {
            return value;
        }
    }
}
