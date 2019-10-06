using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlycoSeqClassLibrary.Algorithm;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide;
using GlycoSeqClassLibrary.Model.Chemistry.Glycan;
using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using GlycoSeqClassLibrary.Model.Chemistry.Peptide;
using GlycoSeqClassLibrary.Model.Spectrum;
using GlycoSeqClassLibrary.Util.CalcMass;

namespace GlycoSeqClassLibrary.Search.Precursor
{
    public class GeneralPrecursorMatcher : IPrecursorMatcher
    {
        List<IPeptide> peptides;
        ISearch matcher;
        IGlycoPeptideCreator creator;

        public GeneralPrecursorMatcher(List<IPeptide> peptides, ISearch matcher, IGlycoPeptideCreator creator)
        {
            this.peptides = peptides;
            this.matcher = matcher;
            this.creator = creator;
        }

        public List<IGlycoPeptide> Match(ISpectrum spectrum, double monoMass)
        {
            List<IGlycoPeptide> result = new List<IGlycoPeptide>();
            double mz = (spectrum as ISpectrumMSn).GetParentMZ();
            int charge = (spectrum as ISpectrumMSn).GetParentCharge();
            double mass = SpectrumCalcMass.Instance.Compute(monoMass, charge);
            foreach (IPeptide peptide in peptides)
            {
                double target = mass - PeptideCalcMass.Instance.Compute(peptide);
                if (target < 0) continue;

                List<IPoint> points = matcher.Search(new GeneralPoint(target));
                foreach(IPoint point in points)
                {
                    result.AddRange(creator.Create((point as GlycanPoint).GetGlycan(), peptide));
                }
            }

            return result;
        }

        public List<IGlycoPeptide> Match(ISpectrum spectrum)
        {
            return Match(spectrum, (spectrum as ISpectrumMSn).GetParentMZ());
        }

        public void SetPeptides(List<IPeptide> peptides)
        {
            this.peptides = peptides;
        }
    }
}
