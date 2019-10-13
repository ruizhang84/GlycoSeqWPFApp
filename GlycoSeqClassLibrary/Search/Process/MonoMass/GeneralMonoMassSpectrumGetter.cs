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
        readonly int scanRange;
        ISearch matcher;
        Dictionary<int, ISpectrum> spectrumTable;

        public GeneralMonoMassSpectrumGetter(ISearch matcher, int maxIsotopic=10, int scanRange=10)
        {
            this.matcher = matcher;
            Proton = IonCalcMass.Hydrogen;
            this.scanRange = scanRange;
            this.maxIsotopic = maxIsotopic;
            // dictionary to store MS1 spectrum
            spectrumTable = new Dictionary<int, ISpectrum>();
        }

        private ISpectrum GetMonoSpectrum(int scanNum)
        {
            for (int scan = scanNum; scan > scanNum - scanRange; scan--)
            {
                if (spectrumTable.ContainsKey(scan))
                {
                    return spectrumTable[scan];
                }
            }
            return null;
        }

        public double GetMonoMass(ISpectrumMSn spectrum)
        {
            double mz = spectrum.GetParentMZ();
            double monoMass = mz;

            // set matcher data
            ISpectrum monoSpectrum = GetMonoSpectrum(spectrum.GetScanNum());
            if (monoSpectrum is null)
            {
                return monoMass;
            }
            else
            {
                List<IPoint> points = new List<IPoint>();
                foreach (IPeak pk in spectrum.GetPeaks())
                {
                    points.Add(new PeakPoint(pk));
                }
                matcher.setData(points);
            }

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
            spectrumTable[spectrum.GetScanNum()] = spectrum;
        }
    }
}
