using GlycoSeqClassLibrary.Model.Chemistry.Glycan;
using GlycoSeqClassLibrary.Model.Chemistry.Glycan.TableNGlycan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Util.CalcMass
{
    public class SingaturePeakCalcMass
    {
        protected static readonly Lazy<SingaturePeakCalcMass>
            lazy = new Lazy<SingaturePeakCalcMass>(() => new SingaturePeakCalcMass());

        public static SingaturePeakCalcMass Instance { get { return lazy.Value; } }

        protected SingaturePeakCalcMass()
        {
        }

        public const double HexNAc = 203.0794;
        public const double Hex = 162.0528;
        public const double Fuc = 146.0579;
        public const double NeuAc = 291.0954;
        public const double NeuGc = 307.0903;

     
        public List<double> ComputeComplex(IGlycan glycan)
        {
            List<double> massList = new List<double>();
            ITableNGlycan nglycan = glycan as ITableNGlycan;
            int[] table = nglycan.GetNGlycanTable();

            double mass = table[4] * HexNAc + table[8] * Hex + table[12] * Fuc + table[16] * NeuAc + table[20] * NeuGc;
            massList.Add(mass);
            massList.Add(mass + Hex);
            massList.Add(mass + Hex * 2);
            massList.Add(mass + Hex * 2 + HexNAc);
            massList.Add(mass + Hex * 2 + HexNAc * 2);
            return massList;
        }


    }
}
