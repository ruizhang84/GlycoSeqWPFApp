using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Model.Spectrum
{
    public class GeneralSpectrumMSn : GeneralSpectrum, ISpectrumMSn
    {
        protected TypeOfMSActivation activationType;
        protected double parentMZ;
        protected int parentCharge;

        public GeneralSpectrumMSn(int msOrder, int scanNum, TypeOfMSActivation type, double mz, int charge) : base(msOrder, scanNum)
        {
            activationType = type;
            parentMZ = mz;
            parentCharge = charge;
        }

        public TypeOfMSActivation GetActivation()
        {
            return activationType;
        }

        public double GetParentMZ()
        {
            return parentMZ;
        }

        public int GetParentCharge()
        {
            return parentCharge;
        }
      
    }
}
