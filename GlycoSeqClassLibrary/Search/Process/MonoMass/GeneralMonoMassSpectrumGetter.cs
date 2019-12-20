using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlycoSeqClassLibrary.Algorithm;
using GlycoSeqClassLibrary.Builder.Spectrum;
using GlycoSeqClassLibrary.Model.Spectrum;
using GlycoSeqClassLibrary.Util.CalcMass;

namespace GlycoSeqClassLibrary.Search.Process.MonoMass
{
    public class GeneralMonoMassSpectrumGetter : IMonoMassSpectrumGetter
    {
        readonly double Proton;
        readonly int maxIsotopic;
        ISearch matcher;

        public GeneralMonoMassSpectrumGetter(ISearch matcher, int maxIsotopic = 10)
        {
            this.matcher = matcher;
            Proton = IonCalcMass.Hydrogen;
            this.maxIsotopic = maxIsotopic;
        }

        // assume the spectrum read sequentially
        public double GetMonoMass(ISpectrum spectrum)
        {
            if (spectrum.GetMSnOrder() == 1)
            {
                List<IPoint> points = new List<IPoint>();
                foreach (IPeak pk in spectrum.GetPeaks())
                {
                    points.Add(new PeakPoint(pk));
                }
                matcher.setData(points);

                return 0;
            }

            ISpectrumMSn spectrumMSn = spectrum as ISpectrumMSn;

            double mz = spectrumMSn.GetParentMZ();
            double monoMass = mz;

           
            // search isotopic point on full MS spectrum
            int charge = spectrumMSn.GetParentCharge();
            int isotopic = 0;
            while (isotopic < maxIsotopic)
            {
                double target = mz - Proton / charge * (isotopic + 1);
                if (!matcher.Found(new GeneralPoint(target))) break;
                isotopic++;
            }

            // get max intensity peak
            if (isotopic == 0)
                return monoMass;
            double isoMZ = mz - Proton / charge * isotopic;
            List<IPoint> matched = matcher.Search(new GeneralPoint(isoMZ));
            return matched.OrderBy(x => Math.Abs((x as PeakPoint).MZ - isoMZ)).First().GetValue();
        }

    }
}