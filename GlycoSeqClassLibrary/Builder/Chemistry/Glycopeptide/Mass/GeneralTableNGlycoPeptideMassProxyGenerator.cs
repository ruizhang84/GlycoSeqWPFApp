using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycan.Mass;
using GlycoSeqClassLibrary.Model.Chemistry.Glycan;
using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using GlycoSeqClassLibrary.Model.Chemistry.Peptide;
using GlycoSeqClassLibrary.Util.CalcMass;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide.Mass
{
    public class GeneralTableNGlycoPeptideMassProxyGenerator : IGlycoPeptideProxyGenerator
    {
        public IGlycoPeptideProxy Generate(IGlycan glycan, IPeptide peptide, int modifySite)
        {
            if (glycan is ITableNGlycanMassProxy)
            {
                IGlycoPeptide glycoPeptide = new GeneralGlycoPeptide(glycan, peptide, modifySite);
                IGlycoPeptideMassProxy glycoPeptideMassProxy = new GeneralGlycoPeptideMassProxy(glycoPeptide);
                foreach(double glycanMass in (glycan as ITableNGlycanMassProxy).GetMass())
                {
                    //glycoPeptideMassProxy.AddMass(glycanMass, MassType.Glycan);
                    glycoPeptideMassProxy.AddMass(glycanMass + PeptideCalcMass.Instance.Compute(glycoPeptide.GetPeptide()), MassType.Glycan);
                    //foreach(double peptideMass in PTMPeptideCalcMass.Compute(peptide.GetSequence(), modifySite))
                    //{
                    //    glycoPeptideMassProxy.AddMass(glycanMass + peptideMass, MassType.Glycan);
                    //}
                }

                double mass = GlycanCalcMass.Instance.Compute(glycan);
                foreach (double peptideMass in PTMPeptideCalcMass.Compute(peptide.GetSequence(), modifySite))
                {
                    glycoPeptideMassProxy.AddMass(mass + peptideMass, MassType.Peptide);
                }

                //glycoPeptideMassProxy.AddRangeMass(PTMPeptideCalcMass.ComputeNonPTM(peptide.GetSequence(), modifySite), MassType.Peptide);

                // reverse Peptide 
                //char[] reversed = peptide.GetSequence().ToCharArray();
                //Array.Reverse(reversed);
                //string reverseSequence = new string(reversed);
                //foreach (double glycanMass in (glycan as ITableNGlycanMassProxy).GetMass())
                //{
                //    foreach (double peptideMass in PTMPeptideCalcMass.Compute(reverseSequence, reverseSequence.Length - 1 - modifySite))
                //    {
                //        glycoPeptideMassProxy.AddMass(glycanMass + peptideMass, MassType.ReverseGlycan);
                //    }
                //}
                //glycoPeptideMassProxy.AddRangeMass(PTMPeptideCalcMass.ComputeNonPTM(reverseSequence, reverseSequence.Length - 1 - modifySite), MassType.ReversePeptide);


                return glycoPeptideMassProxy;

            }
            else
            {
                throw new InvalidCastException("Can not cast to ITableNGlycanMassProxy");
            }
        }
    }
}
