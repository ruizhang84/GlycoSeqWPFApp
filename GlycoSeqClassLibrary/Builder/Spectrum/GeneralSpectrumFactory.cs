using GlycoSeqClassLibrary.Model.Spectrum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Builder.Spectrum
{
    public class GeneralSpectrumFactory : ISpectrumFactory
    {
        public ISpectrumReader reader;
        public GeneralSpectrumFactory(ISpectrumReader reader)
        {
            this.reader = reader;
        }

        public TypeOfMSActivation GetActivation(int scanNum)
        {
            return reader.GetActivation(scanNum);
        }

        public int GetFirstScan()
        {
            return reader.GetFirstScan();
        }

        public int GetLastScan()
        {
            return reader.GetLastScan();
        }

        public int GetMSnOrder(int scanNum)
        {
            return reader.GetMSnOrder(scanNum);
        }

        public ISpectrum GetSpectrum(int scanNum)
        {
            try
            {
                if (reader.GetMSnOrder(scanNum) == 1)
                {
                    GeneralSpectrum spectrum = new GeneralSpectrum(1, scanNum);
                    List<IPeak> peaks = reader.Read(scanNum);
                    spectrum.SetPeaks(peaks);
                    return spectrum;
                }
                else
                {
                    int msOrder = reader.GetMSnOrder(scanNum);
                    TypeOfMSActivation type = reader.GetActivation(scanNum);
                    double[] mzCharge = reader.GetParentMZCharge(scanNum);
                    ISpectrum spectrum = new GeneralSpectrumMSn(msOrder, scanNum, type, mzCharge[0], (int)mzCharge[1]);

                    List<IPeak> peaks = reader.Read(scanNum);
                    spectrum.SetPeaks(peaks);
                    return spectrum;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }

        public void Init(string fileName)
        {
            reader.Init(fileName);
        }
    }
}
