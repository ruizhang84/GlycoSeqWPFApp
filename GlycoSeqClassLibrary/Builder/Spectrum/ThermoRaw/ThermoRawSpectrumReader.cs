using GlycoSeqClassLibrary.Model.Spectrum;
using MSFileReaderLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThermolRawClassLibrary;

namespace GlycoSeqClassLibrary.Builder.Spectrum.ThermoRaw
{
    public class ThermoRawSpectrumReader : ISpectrumReader
    {
        protected IXRawfile5 rawConnect;
        protected RawReader rawReader;

        public ThermoRawSpectrumReader(string fileName)
        {
            rawConnect = new MSFileReader_XRawfile() as IXRawfile5;
            rawReader = new RawReader();

            rawConnect.Open(fileName);
            rawConnect.SetCurrentController(0, 1);
            rawReader.Init(fileName);
        }

        ~ThermoRawSpectrumReader()
        {
            rawConnect.Close();
        }

        public int GetFirstScan()
        {
            int startScanNum = 0;
            rawConnect.GetFirstSpectrumNumber(ref startScanNum);

            return startScanNum;
        }

        public int GetLastScan()
        {
            int lastScanNum = 0;
            rawConnect.GetLastSpectrumNumber(ref lastScanNum);
            return lastScanNum;
        }

        public int GetMSnOrder(int scanNum)
        {
            int msnOrder = 0;
            rawConnect.GetMSOrderForScanNum(scanNum, ref msnOrder);
            return msnOrder;
        }

        public List<IPeak> Read(int scanNum)
        {
            string szFilter = "";
            int pnScanNumber = scanNum;
            int nIntensityCutoffType = 0;
            int nIntensityCutoffValue = 0;
            int nMaxNumberOfPeaks = 0;
            int bCentroidResult = 0;
            int pnArraySize = 0;
            double pdCentroidPeakWidth = 0;
            object pvarMassList = null;
            object pvarPeakFlags = null;

            rawConnect.GetMassListFromScanNum(ref pnScanNumber, szFilter, nIntensityCutoffType, nIntensityCutoffValue,
                nMaxNumberOfPeaks, bCentroidResult, ref pdCentroidPeakWidth, ref pvarMassList, ref pvarPeakFlags, ref pnArraySize);

            ////construct peaks
            double[,] value = (double[,])pvarMassList;
            List<IPeak> peaks = new List<IPeak>();
            for (int pn = 0; pn < pnArraySize; pn++)
            {
                double mass = value[0, pn];
                double intensity = value[1, pn];
                if (intensity > 0)
                {
                    peaks.Add(new GeneralPeak(mass, intensity));
                }
            }
            return peaks;
        }

        public TypeOfMSActivation GetActivation(int scanNum)
        {
            int pnActivationType = 0;
            rawConnect.GetActivationTypeForScanNum(scanNum, GetMSnOrder(scanNum), ref pnActivationType);
            return (TypeOfMSActivation) pnActivationType;
        }

        public double[] GetParentMZCharge(int scanNum)
        {
            return rawReader.GetPrecursorInfo(scanNum);
        }

    }
}
