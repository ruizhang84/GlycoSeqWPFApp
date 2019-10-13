using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThermoDLL;

namespace ThermolRawClassLibrary
{
    public class RawReader
    {
        protected string path;
        protected ThermoDLLClass raws;

        public RawReader()
        {
            raws = new ThermoDLLClass();
        }
        public void Init(string path)
        {
            this.path = path;
        }
        public double[] GetPrecursorInfo(int scanNum)
        {
            PrecursorInfo infos = raws.GetPrecursorInfo(scanNum, path);
            double[] mzCharge = new double[2];

            double mz = infos.dMonoIsoMass;
            int charge = infos.nChargeState;

            mzCharge[0] = mz;
            mzCharge[1] = charge;
            return mzCharge;
        }

    }
}
