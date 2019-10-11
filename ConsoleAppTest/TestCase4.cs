using GlycoSeqClassLibrary.Builder.Spectrum;
using GlycoSeqClassLibrary.Builder.Spectrum.ThermoRaw;
using GlycoSeqClassLibrary.Model.Spectrum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    public class TestCase4: TestCase
    {
        public void Run()
        {
            ISpectrumReader reader = new ThermoRawSpectrumReader();
            reader.Init(@"C:\Users\iruiz\Desktop\app\ZC_20171218_H95_R1.raw");
            ISpectrumFactory creator = new GeneralSpectrumFactory(reader);

            Console.WriteLine(creator.GetFirstScan());
            Console.WriteLine(creator.GetLastScan());

            for(int i = creator.GetFirstScan(); i < creator.GetLastScan(); i++)
            {
                if (creator.GetMSnOrder(i) == 2)
                {
                    Console.WriteLine(creator.GetActivation(i).ToString());
                    List<IPeak> peaks = creator.GetSpectrum(i).GetPeaks();
                }
             
            }
            Console.Read();
        }

    }
}
