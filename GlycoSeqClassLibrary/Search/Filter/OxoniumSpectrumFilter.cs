using GlycoSeqClassLibrary.Algorithm;
using GlycoSeqClassLibrary.Model.Spectrum;
using GlycoSeqClassLibrary.Util.CalcMass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Search.Filter
{
    // Only HCD MS/MS spectra containing HexNAc oxonium ions were annotated
    // ref: Parker B.L., Thaysen-Andersen M., Solis N., Scott N. E., Larsen M. R., Graham M. E., Packer N. H., and Cordwell S.J. (2013)
    // Site-specific glycan-peptide analysis for determination of N-glycoproteome heterogeneity.J.Proteome Res.12, 5791–5800

    public class OxoniumSpectrumFilter : ISpectrumFilter
    {
        ISearch matcher;
        public OxoniumSpectrumFilter(ISearch matcher)
        {
            this.matcher = matcher;
        }

        private void SetMatcher(ISpectrum spectrum)
        {
            List<IPoint> points = new List<IPoint>();
            foreach(IPeak peaks in spectrum.GetPeaks())
            {
                points.Add(new GeneralPoint(peaks.GetMZ()));
            }
            matcher.setData(points);
        }

        public bool Filter(ISpectrum spectrum)
        {
            if (spectrum.GetMSnOrder() > 1)
            {
                SetMatcher(spectrum);
                int charge = (spectrum as ISpectrumMSn).GetParentCharge();
                for(int z = 1; z <= charge; z++)
                {
                    double mz = SpectrumCalcMass.Instance.ComputeMZ(GlycanCalcMass.HexNAc, z);
                    if (matcher.Found(new GeneralPoint(mz)))
                    {
                        return false;
                    }
                }

                return  true;
            }

            return false;
        }

    }
}
