using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlycoSeqClassLibrary.Algorithm;
using GlycoSeqClassLibrary.Model.Spectrum;
using GlycoSeqClassLibrary.Util.CalcMass;

namespace GlycoSeqClassLibrary.Search.Process.MonoMass
{
    public class GeneralMonoMassSpectrumGetter : IMonoMassSpectrumGetter
    {
        readonly double Proton;
        readonly int maxIsotopic;
        ISearch matcher;

        public GeneralMonoMassSpectrumGetter(ISearch matcher, ISpectrum spectrum=null, int maxIsotopic=10)
        {
            this.matcher = matcher;
            Proton = IonCalcMass.Hydrogen;
            this.maxIsotopic = maxIsotopic;
            
            if (spectrum != null)
            {
                List<IPoint> points = new List<IPoint>();
                foreach (IPeak pk in spectrum.GetPeaks())
                {
                    points.Add(new PeakPoint(pk));
                }
                matcher.setData(points);
            } 
        }

        public double GetMonoMass(ISpectrumMSn spectrum)
        {
            double mz = spectrum.GetParentMZ();
            double monoMass = mz;

            // search isotopic point on full MS spectrum
            int charge = spectrum.GetParentCharge();
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
            return matched.OrderBy(x => (x as PeakPoint).Intensity).First().GetValue();
        }

        public void SetMonoMassSpectrum(ISpectrum spectrum)
        {
            List<IPoint> points = new List<IPoint>();
            foreach (IPeak pk in spectrum.GetPeaks())
            {
                points.Add(new PeakPoint(pk));
            }
            matcher.setData(points);
        }
    }
}
