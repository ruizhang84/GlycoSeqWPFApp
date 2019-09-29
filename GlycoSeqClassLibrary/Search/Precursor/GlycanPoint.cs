using GlycoSeqClassLibrary.Algorithm;
using GlycoSeqClassLibrary.Model.Chemistry.Glycan;
using GlycoSeqClassLibrary.Util.CalcMass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Search.Precursor
{
    public class GlycanPoint : IPoint
    {
        IGlycan glycan;
        double mass;

        public GlycanPoint(IGlycan glycan)
        {
            this.glycan = glycan;
            mass = GlycanCalcMass.Instance.Compute(glycan);
        }

        public IGlycan GetGlycan()
        {
            return glycan;
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
