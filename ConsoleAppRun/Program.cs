using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
using GlycoSeqClassLibrary.Builder.Spectrum;
using GlycoSeqClassLibrary.Engine.EngineSetup.Spectrum;
using GlycoSeqClassLibrary.Model.Spectrum;

namespace ConsoleAppRun
{
    public class Program
    {
        public static int GetScan(string name)
        {
            MatchCollection mc = Regex.Matches(name, @"scan=(\d+)");
            int scan = -1;
            foreach (Match m in mc)
            {
                GroupCollection data = m.Groups;

                int.TryParse(data[1].ToString(), out scan);
                return scan;
            }
            return scan;
        }

        public static void ReadCSV(string fileName, Dictionary<string, List<int>> scanInfo)
        {
            // read csv
            using (var reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    string key = values[3] + values[5];
                    if (!scanInfo.ContainsKey(key))
                    {
                        scanInfo[key] = new List<int>();
                    }
                    scanInfo[key].Add(GetScan(values[24]));
                }
            }
        }

        public class SpectramInfo
        {
            public ISpectrum Spectrum { get; set; }
            public int ScanNum { get; set; }
            public string FileName { get; set; }
            public SpectramInfo(ISpectrum spectrum, int scan, string file)
            {
                Spectrum = spectrum;
                ScanNum = scan;
                FileName = file;
            }
        }

        public static void ReadRaw(string fileName, Dictionary<string, List<int>> scanInfo,
            Dictionary<string, List<SpectramInfo>> spectraInfo)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new TopPeakPickingDelegatorModule() { MaxPeaks = 100 });
            builder.RegisterModule(new SpectrumProcessingModule() { ScaleFactor = 1.0 });
            builder.RegisterModule(new ThermoRawSpectrumModule());

            IContainer Container = builder.Build();
            SpectrumCosineSimilarity calculator = new SpectrumCosineSimilarity();

            using (var scope = Container.BeginLifetimeScope())
            {
                var spectrumFacotry = scope.Resolve<ISpectrumFactory>();
                spectrumFacotry.Init(fileName);
                foreach (string key in scanInfo.Keys)
                {
                    List<int> scans = scanInfo[key];
                    foreach(int scan in scans)
                    {
                        if (!spectraInfo.ContainsKey(key))
                        {
                            spectraInfo[key] = new List<SpectramInfo>();
                        }
                        spectraInfo[key].Add(new SpectramInfo(spectrumFacotry.GetSpectrum(scan), scan, fileName));
                    }
                    
                }
            }

        }

        public class CosInfo
        {
            public string Name { get; set; }
            public string FileA { get; set; }
            public string FileB { get; set; }
            public int ScanA { get; set; }
            public int ScanB { get; set; }
            public double cos { get; set; }
            public CosInfo(string Name, int ScanA, int ScanB, string FileA, string FileB, double cos)
            {
                this.Name = Name;
                this.ScanA = ScanA;
                this.ScanB = ScanB;
                this.FileA = FileA;
                this.FileB = FileB;
                this.cos = cos;
            }
        }

        public static void CosCompute(List<CosInfo> cosInfos, 
            Dictionary<string, List<SpectramInfo>> spectraInfo, double tol = 0.01)
        {
            SpectrumCosineSimilarity calculator = new SpectrumCosineSimilarity();
            foreach (string key in spectraInfo.Keys)
            {
                List<SpectramInfo> spectra = spectraInfo[key];
                if (spectra.Count < 2) continue;
                for (int i = 0; i < spectra.Count - 1; i++)
                {

                    for (int j = i + 1; j < spectra.Count; j++)
                    {
                        ISpectrum spectrumA = spectra[i].Spectrum;
                        ISpectrum spectrumB = spectra[j].Spectrum;
                        double cons = calculator.computeCos(spectrumA, spectrumB, tol);
                        cosInfos.Add(new CosInfo(key, spectra[i].ScanNum, spectra[j].ScanNum, 
                            spectra[i].FileName, spectra[j].FileName, cons));
                    }
                }
            }
            
        }

        public static void WriteCSV(string output, List<CosInfo> cosInfos, 
            double cutoff = 0.7)
        {
            try
            {
                FileStream ostrm = new FileStream(output, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter writer = new StreamWriter(ostrm);
                writer.Write("Glycopeptide, ");
                writer.Write("SpectrumA: ScanA, ");
                writer.Write("SpectrumB: ScanB, ");
                writer.Write("Cosine, ");
                writer.WriteLine();
                foreach (CosInfo info in cosInfos)
                {
                    if (info.cos < cutoff)
                    {
                        writer.Write(info.Name);
                        writer.Write(",");
                        writer.Write(info.FileA + " : " + info.ScanA.ToString());
                        writer.Write(",");
                        writer.Write(info.FileB + " : " + info.ScanB.ToString());
                        writer.Write(",");
                        writer.Write(info.cos);
                        writer.Write(",");
                        writer.WriteLine();
                    }
                }
                writer.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open file!");
                Console.WriteLine(e.Message);
            }
        }

        public static void Run(List<string> files)
        {
            string dir = @"C:\Users\iruiz\Desktop\app3\";
            
            List<CosInfo> cosInfos = new List<CosInfo>();
            Dictionary<string, List<SpectramInfo>> spectraInfo = new Dictionary<string, List<SpectramInfo>>();
            foreach (string name in files)//string name = @"H96_R2";
            {
                Dictionary<string, List<int>> scanInfos = new Dictionary<string, List<int>>();
                ReadCSV(dir + name + "_Byonic.csv", scanInfos);
                ReadRaw(dir + @"ZC_20171218_" + name + ".raw", scanInfos, spectraInfo);
            }

            CosCompute(cosInfos, spectraInfo, 0.01);
            WriteCSV(dir + "result_cos.csv", cosInfos, 0.65);
           
        }

        static void Main(string[] args)
        {
            List<string> files = new List<string>()
            {
                @"H68_R1", @"H68_R2",
                @"H84_R1", @"H84_R2",
                @"H89_R1", @"H89_R2",
                @"H95_R1", @"H95_R2",
                @"H96_R1", @"H96_R2"
            };
            Run(files);
            

        }
    }
}
