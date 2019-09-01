using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Util.CalcMass
{
    public class SpectrumCalcMass
    {
        protected static readonly Lazy<SpectrumCalcMass>
            lazy = new Lazy<SpectrumCalcMass>(() => new SpectrumCalcMass());

        public static SpectrumCalcMass Instance { get { return lazy.Value; } }

        protected SpectrumCalcMass()
        {
            ion = 1.0078;
        }

        protected double ion;

        public void SetChargeIon(double ionMass)
        {
            ion = ionMass;
        }

        public double Compute(double mz, int charge)
        {
            return (mz - ion) * charge;
        }

        public double ComputePPM(double expected, double observed)
        {
            return Math.Abs(expected - observed) / expected * 1000000.0;
        }

    }
}
