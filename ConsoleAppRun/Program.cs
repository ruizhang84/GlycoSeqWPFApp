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
    class Program
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

        public class  CosInfo
        {
            public string Name { get; set; }
            public int ScanA { get; set; }
            public int ScanB { get; set; }
            public double cos { get; set; }
            public CosInfo(string Name, int ScanA, int ScanB, double cos)
            {
                this.Name = Name;
                this.ScanA = ScanA;
                this.ScanB = ScanB;
                this.cos = cos;
            }
        }

        public static void CosCompute(string fileName, 
            List<CosInfo> cosInfos, Dictionary<string, List<int>> scanInfo, double tol = 0.01)
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
                    if (scans.Count < 2) continue;
                    for (int i = 0; i < scans.Count - 1; i++)
                    {

                        for (int j = i + 1; j < scans.Count; j++)
                        {
                            ISpectrum spectrumA = spectrumFacotry.GetSpectrum(scans[i]);
                            ISpectrum spectrumB = spectrumFacotry.GetSpectrum(scans[j]);
                            double cons = calculator.computeCos(spectrumA, spectrumB, tol);
                            cosInfos.Add(new CosInfo(key, scans[i], scans[j], cons));
                        }
                    }
                }
            }
        }

        public static void WriteCSV(string output, List<CosInfo> cosInfos)
        {
            try
            {
                FileStream ostrm = new FileStream(output, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter writer = new StreamWriter(ostrm);
                writer.Write("Glycopeptide, ");
                writer.Write("ScanA, ");
                writer.Write("ScanB, ");
                writer.Write("Cosine, ");
                writer.WriteLine();
                foreach(CosInfo info in cosInfos)
                {
                    writer.Write(info.Name);
                    writer.Write(",");
                    writer.Write(info.ScanA);
                    writer.Write(",");
                    writer.Write(info.ScanB);
                    writer.Write(",");
                    writer.Write(info.cos);
                    writer.Write(",");
                    writer.WriteLine();
                }
                writer.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open file!");
                Console.WriteLine(e.Message);
            }
        }

        public static int GetParent(int num, Dictionary<int, int> parent)
        {
            if (parent[num] == num) return num;
            return GetParent(parent[num], parent);
        }
        public static void GetCluster(List<CosInfo> cosInfos,
            Dictionary<int, List<int>> clusters,
            Dictionary<int, string> names,
            double cutoff = 0.7)
        {
            Dictionary<int, int> parent = new Dictionary<int, int>();
            Dictionary<int, int> rank = new Dictionary<int, int>();
            

            foreach (CosInfo info in cosInfos)
            {
                if (info.cos >= cutoff)
                {
                    if (!parent.ContainsKey(info.ScanA))
                    {
                        parent[info.ScanA] = info.ScanA;
                        names[info.ScanA] = info.Name;
                        rank[info.ScanA] = 1;
                    }
                    if (!parent.ContainsKey(info.ScanB))
                    {
                        parent[info.ScanB] = info.ScanB;
                        names[info.ScanB] = info.Name;
                        rank[info.ScanB] = 1;
                    }

                    // not in the same group, merge
                    int parentA = GetParent(info.ScanA, parent);
                    int parentB = GetParent(info.ScanB, parent);
                    if (parentA != parentB)
                    {
                       if (rank[parentA] > rank[parentB])
                        {
                            parent[parentA] = parentB;
                        }
                        else
                        {
                            if (rank[parentA] == rank[parentB])
                            {
                                rank[parentA]++;
                            }
                            parent[parentB] = parentA;
                        }
                    }
                }
            }
            
            foreach(int scan in parent.Keys)
            {
                int p = GetParent(scan, parent);
                if (!clusters.ContainsKey(p))
                {
                    clusters[p] = new List<int>();
                }
                clusters[p].Add(scan);
            }
        }
        public static void WriteClusterCSV(string output, 
            Dictionary<int, List<int>> clusters, Dictionary<int, string> names)
        {
            try
            {
                FileStream ostrm = new FileStream(output, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter writer = new StreamWriter(ostrm);
                writer.Write("Glycopeptide, ");
                writer.Write("Scans, ");
                writer.WriteLine();
                foreach (int i in clusters.Keys)
                {
                    if (clusters[i].Count < 2) continue;
                    writer.Write(names[i]);
                    writer.Write(",");
                    writer.Write(string.Join(";", clusters[i]));
                    writer.Write(",");
                    writer.WriteLine();
                }
                writer.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open file!");
                Console.WriteLine(e.Message);
            }
        }

        static void Main(string[] args)
        {
            string dir = @"C:\Users\iruiz\Desktop\app3\";
            string name = @"H96_R2";

            Dictionary<string, List<int>> scanInfos = new Dictionary<string, List<int>>();
            ReadCSV(dir + name + "_Byonic.csv", scanInfos);

            List<CosInfo> cosInfos = new List<CosInfo>();
            CosCompute(dir + @"\ZC_20171218_" + name + ".raw", cosInfos, scanInfos);

            WriteCSV(dir + name + "_cos.csv", cosInfos);

            Dictionary<int, List<int>> clusters = new Dictionary<int, List<int>>();
            Dictionary<int, string> names = new Dictionary<int, string>();
            GetCluster(cosInfos, clusters, names);

            WriteClusterCSV(dir + name + "_cluster.csv", clusters, names);
        }
    }
}
