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
                    glycoPeptideMassProxy.AddMass(glycanMass + 
                        PeptideCalcMass.Instance.Compute(glycoPeptide.GetPeptide()), MassType.Glycan);
                }

                foreach (double glycanMass in (glycan as ITableNGlycanMassProxy).GetCoreMass())
                {
                    glycoPeptideMassProxy.AddMass(glycanMass + 
                        PeptideCalcMass.Instance.Compute(glycoPeptide.GetPeptide()), MassType.CoreGlycan);
                }

                double mass = GlycanCalcMass.Instance.Compute(glycan);
                foreach (double peptideMass in PTMPeptideCalcMass.Compute(peptide.GetSequence(), modifySite))
                {
                    glycoPeptideMassProxy.AddMass(mass + peptideMass, MassType.Peptide);
                }

                return glycoPeptideMassProxy;

            }
            else
            {
                throw new InvalidCastException("Can not cast to ITableNGlycanMassProxy");
            }
        }
    }
}
