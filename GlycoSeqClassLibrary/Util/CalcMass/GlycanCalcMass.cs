using GlycoSeqClassLibrary.Model.Chemistry.Glycan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Util.CalcMass
{
    public class GlycanCalcMass
    {
        protected static readonly Lazy<GlycanCalcMass>
        lazy = new Lazy<GlycanCalcMass>(() => new GlycanCalcMass());

        public static GlycanCalcMass Instance { get { return lazy.Value; } }

        protected GlycanCalcMass()
        {
        }

        public const double HexNAc = 203.0794;
        public const double Hex = 162.0528;
        public const double Fuc = 146.0579;
        public const double NeuAc = 291.0954;
        public const double NeuGc = 307.0903;

        public const double PermHexNAc = 245.1263;
        public const double PermHex = 204.0998;
        public const double PermFuc = 174.0892;
        public const double PermNeuAc = 361.1737;  //N-acetyl-neuraminic acid
        public const double PermNeuGc = 391.1842;  //N-glycolyl-neuraminic acid

        public bool permethylation;
        public void SetPermethylation(bool set)
        {
            permethylation = set;
        }

        private double CalcSingleGlycanMass(IGlycan glycan)
        {
            int[] compos = glycan.GetStructure();
            return compos[0] * HexNAc + compos[1] * Hex +
                compos[2] * Fuc + compos[3] * NeuAc + compos[4] * NeuGc;
        }

        private double CalcPermGlycanMass(IGlycan glycan)
        {
            int[] compos = glycan.GetStructure();
            return compos[0] * PermHexNAc + compos[1] * PermHex +
                compos[2] * PermFuc + compos[3] * PermNeuAc + compos[4] * PermNeuGc;
        }

        public double Compute(IGlycan glycan)
        {
            if (permethylation)
                return CalcPermGlycanMass(glycan);
            return CalcSingleGlycanMass(glycan);
        }
    }
}
