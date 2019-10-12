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
                    foreach(double peptideMass in PTMPeptideCalcMass.Compute(peptide.GetSequence(), modifySite))
                    {
                        glycoPeptideMassProxy.AddMass(glycanMass + peptideMass, MassType.Glycan);
                    }
                }
                glycoPeptideMassProxy.AddRangeMass(PTMPeptideCalcMass.ComputeNonPTM(peptide.GetSequence(), modifySite), MassType.Peptide);

                return glycoPeptideMassProxy;

            }
            else
            {
                throw new InvalidCastException("Can not cast to ITableNGlycanMassProxy");
            }
        }
    }
}
