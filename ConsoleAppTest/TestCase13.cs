using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
using GlycoSeqClassLibrary.Analyze;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycan;
using GlycoSeqClassLibrary.Builder.Chemistry.Peptide;
using GlycoSeqClassLibrary.Builder.Chemistry.Protein;
using GlycoSeqClassLibrary.Builder.Spectrum;
using GlycoSeqClassLibrary.Engine;
using GlycoSeqClassLibrary.Engine.EngineSetup.Glycan;
using GlycoSeqClassLibrary.Engine.EngineSetup.GlycoPeptide;
using GlycoSeqClassLibrary.Engine.EngineSetup.Peptide;
using GlycoSeqClassLibrary.Engine.EngineSetup.Protein;
using GlycoSeqClassLibrary.Engine.EngineSetup.Report;
using GlycoSeqClassLibrary.Engine.EngineSetup.Search;
using GlycoSeqClassLibrary.Engine.EngineSetup.Spectrum;
using GlycoSeqClassLibrary.Engine.SearchEThcD;
using GlycoSeqClassLibrary.Model.Spectrum;
using GlycoSeqClassLibrary.Search.Filter;
using GlycoSeqClassLibrary.Search.Precursor;
using GlycoSeqClassLibrary.Search.Process;
using GlycoSeqClassLibrary.Search.SearchEThcD;

namespace ConsoleAppTest
{
    public class TestCase13 : TestCase
    {
        public double computeDot(List<IPeak> l1, List<IPeak> l2)
        {
            if (l1.Count == 0 || l2.Count == 0)
            {
                return 0;
            }
            int count = Math.Min(l1.Count, l2.Count);
            List<IPeak> t1 = l1.OrderByDescending(x => x.GetIntensity())
                .Take(count).OrderBy(x => x.GetMZ()).ToList();
            List<IPeak> t2 = l2.OrderByDescending(x => x.GetIntensity()).
                Take(count).OrderBy(x => x.GetMZ()).ToList();
            double numerator = 0;
            for (int i = 0; i < t1.Count; i++)
            {
                numerator += t1[i].GetIntensity() * t2[i].GetIntensity();
            }
            return numerator;
        }

        public double computeCos(List<IPeak> p1, List<IPeak> p2, double tol)
        {
            double lowerBound = Math.Min(p1.Min(x => x.GetMZ()), p2.Min(x => x.GetMZ()));
            double upperBound = Math.Max(p1.Max(x => x.GetMZ()), p2.Max(x => x.GetMZ()));
            int bucketNums = (int)Math.Ceiling((upperBound - lowerBound + 1) / tol);

            List<IPeak>[] q1 = new List<IPeak>[bucketNums];
            List<IPeak>[] q2 = new List<IPeak>[bucketNums];

            for (int i = 0; i < bucketNums; i++)
            {
                q1[i] = new List<IPeak>();
                q2[i] = new List<IPeak>();
            }

            foreach (IPeak pk in p1)
            {
                int index = (int)Math.Ceiling((pk.GetMZ() - lowerBound) / tol);
                q1[index].Add(pk);
            }
            foreach (IPeak pk in p2)
            {
                int index = (int)Math.Ceiling((pk.GetMZ() - lowerBound) / tol);
                q2[index].Add(pk);
            }

            double numerator = 0;
            for (int i = 0; i < bucketNums; i++)
            {
                numerator += computeDot(q1[i], q2[i]);
            }

            double denominator1 = 0;
            foreach (IPeak pk in p1)
            {
                denominator1 += pk.GetIntensity() * pk.GetIntensity();
            }
            double denominator2 = 0;
            foreach (IPeak pk in p2)
            {
                denominator2 += pk.GetIntensity() * pk.GetIntensity();
            }
            double denominator = Math.Sqrt(denominator1) * Math.Sqrt(denominator2);
            return numerator / denominator;
        }


        public void Run()
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            var builder = new ContainerBuilder();
            builder.RegisterModule(new TopPeakPickingDelegatorModule() { MaxPeaks = 100 });
            builder.RegisterModule(new SpectrumProcessingModule() { ScaleFactor = 1.0 });
            builder.RegisterModule(new ThermoRawSpectrumModule());

            IContainer Container = builder.Build();

            List<IPeak> peakA = new List<IPeak>();
            List<IPeak> peakB = new List<IPeak>();

            using (var scope = Container.BeginLifetimeScope())
            {
                var spectrumFacotry = scope.Resolve<ISpectrumFactory>();
                spectrumFacotry.Init(@"C:\Users\Rui Zhang\Downloads\ZC_20171218_H95_R1.raw");

                ISpectrum spectrum = spectrumFacotry.GetSpectrum(8140);
                peakA.AddRange(spectrum.GetPeaks());
            }

            using (var scope = Container.BeginLifetimeScope())
            {
                var spectrumFacotry = scope.Resolve<ISpectrumFactory>();
                spectrumFacotry.Init(@"C:\Users\Rui Zhang\Downloads\ZC_20171218_H68_R1.raw");

                ISpectrum spectrum = spectrumFacotry.GetSpectrum(8081);
                peakB.AddRange(spectrum.GetPeaks());
            }

            double cons = computeCos(peakA, peakB, 0.01);
            if (cons < 0.3)
            {
                Console.WriteLine("spectrum: " + cons.ToString());
                Console.WriteLine(cons);
            }

            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
            Console.Read();
        }

    }
}
