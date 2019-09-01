using GlycoSeqClassLibrary.Model.Chemistry.Glycan;
using GlycoSeqClassLibrary.Model.Chemistry.Peptide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Util.CalcMass
{
    public enum IonType { aIon, bIon, cIon, xIon, yIon, zIon };

    public class IonCalcMass
    {
        protected static readonly Lazy<IonCalcMass>
        lazy = new Lazy<IonCalcMass>(() => new IonCalcMass());

        public static IonCalcMass Instance { get { return lazy.Value; } }

        protected IonCalcMass()
        {
        }

        public const double Carbon = 12.0;
        public const double Nitrogen = 14.003074;
        public const double Oxygen = 15.99491463;
        public const double Hydrogen = 1.007825;

        public double Compute(IGlycan glycan, IonType type)
        {
            double mass = GlycanCalcMass.Instance.Compute(glycan);
            switch (type)
            {
                case IonType.yIon:
                    mass += Oxygen + Hydrogen * 2;
                break;
            }
            return mass;
        }

        public double Compute(IPeptide peptide, IonType type)
        {
            double mass = PeptideCalcMass.Instance.Compute(peptide); //with an addtional h2o
            switch (type)
            {
                case IonType.aIon:
                    mass = mass - Oxygen * 2 - Hydrogen - Carbon;
                    break;
                case IonType.bIon:
                    mass = mass - Oxygen - Hydrogen;
                    break;
                case IonType.cIon:
                    mass = mass - Oxygen + Hydrogen * 2 + Nitrogen;// + 0.02337;//- Oxygen + Hydrogen * 2 + Nitrogen;
                    break;
                case IonType.xIon:
                    mass += Carbon + Oxygen - Hydrogen;
                    break;
                case IonType.yIon:
                    mass += Hydrogen;
                    break;
                case IonType.zIon:
                    mass = mass - Nitrogen - Hydrogen * 2; //- 16.01807;//- Nitrogen - Hydrogen * 2;
                    break;
            }
            return mass;
        }
    }
}
