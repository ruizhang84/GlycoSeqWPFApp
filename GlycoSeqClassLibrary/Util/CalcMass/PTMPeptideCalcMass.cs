using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Util.CalcMass
{
    public class PTMPeptideCalcMass
    {
        public static List<double> Compute(string seq, int pos)
        {
            List<double> massList = new List<double>();
            //massList.Add(PeptideCalcMass.Instance.Compute(seq));
            for (int i = pos; i < seq.Length - 1; i++) // seldom at n
            {

                double mass = IonCalcMass.Instance.ComputePeptide(seq.Substring(0, i + 1), IonType.cIon);
                massList.Add(mass);
            }
            for (int i = 1; i <= pos; i++)
            {
                double mass = IonCalcMass.Instance.ComputePeptide(seq.Substring(i, seq.Length - i), IonType.zIon);
                massList.Add(mass);
            }
            return massList;
        }

        public static List<double> ComputeNonPTM(string seq, int pos)
        {
            List<double> massList = new List<double>();
            //massList.Add(PeptideCalcMass.Instance.Compute(seq));
            for (int i = 0; i < pos ; i++) // seldom at n
            {

                double mass = IonCalcMass.Instance.ComputePeptide(seq.Substring(0, i + 1), IonType.cIon);
                massList.Add(mass);
            }
            for (int i = pos + 1; i < seq.Length; i++)
            {
                double mass = IonCalcMass.Instance.ComputePeptide(seq.Substring(i, seq.Length - i), IonType.zIon);
                massList.Add(mass);
            }
            return massList;
        }
    }
}
