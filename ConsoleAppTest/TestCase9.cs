//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Autofac;
//using GlycoSeqClassLibrary.Analyze;
//using GlycoSeqClassLibrary.Builder.Chemistry.Glycan;
//using GlycoSeqClassLibrary.Builder.Chemistry.Peptide;
//using GlycoSeqClassLibrary.Builder.Chemistry.Protein;
//using GlycoSeqClassLibrary.Builder.Spectrum;
//using GlycoSeqClassLibrary.Engine;
//using GlycoSeqClassLibrary.Engine.EngineSetup.Glycan;
//using GlycoSeqClassLibrary.Engine.EngineSetup.GlycoPeptide;
//using GlycoSeqClassLibrary.Engine.EngineSetup.Peptide;
//using GlycoSeqClassLibrary.Engine.EngineSetup.Protein;
//using GlycoSeqClassLibrary.Engine.EngineSetup.Report;
//using GlycoSeqClassLibrary.Engine.EngineSetup.Search;
//using GlycoSeqClassLibrary.Engine.EngineSetup.Spectrum;
//using GlycoSeqClassLibrary.Engine.SearchEThcD;
//using GlycoSeqClassLibrary.Search.Precursor;
//using GlycoSeqClassLibrary.Search.Process;
//using GlycoSeqClassLibrary.Search.SearchEThcD;

//namespace ConsoleAppTest
//{
//    public class TestCase9 : TestCase
//    {
//        public void Run()
//        {
//            var watch = new System.Diagnostics.Stopwatch();
//            watch.Start();

//            var builder = new ContainerBuilder();
//            builder.RegisterModule(new NGlycanModule() { HexNAcBound = 12, HexBound =12, FucBound = 5, NeuAcBound = 4, NeuGcBound =0 });
//            builder.RegisterModule(new NGlycoPeptideModule());

//            List<string> enzymes = new List<string>() {"GluC", "Trypsin" };
//            builder.RegisterModule(new DoubleDigestionPeptidesModule() { Enzymes = enzymes, MiniLength = 7, MissCleavage = 2 });

//            builder.RegisterModule(new FastaProteinModule());
//            builder.RegisterModule(new CSVReportModule());

//            builder.RegisterModule(new MonoMassSpectrumGetterModule() { Tolerance = 5} );
//            builder.RegisterModule(new PrecursorMatcherModule() { Tolerance = 20 });
//            builder.RegisterModule(new SearchEThcDModule() { Tolerance = 0.01 });

//            builder.RegisterModule(new TopPeakPickingDelegatorModule() { MaxPeaks = 100});
//            builder.RegisterModule(new SpectrumProcessingModule());
//            builder.RegisterModule(new ThermoRawSpectrumModule());

//            builder.Register(c => new GeneralSearchEThcDEngine(c.Resolve<IProteinCreator>(), c.Resolve<IPeptideCreator>(),
//                c.Resolve<IGlycanCreator>(), c.Resolve<ISpectrumFactory>(), c.Resolve<ISpectrumProcessing>(),
//                c.Resolve<IMonoMassSpectrumGetter>(), c.Resolve<IPrecursorMatcher>(), c.Resolve<ISearchEThcD>(),
//                c.Resolve<IResults>(), c.Resolve<IReportProducer>())).As<ISearchEngine>();

//            IContainer Container = builder.Build();

//            using (var scope = Container.BeginLifetimeScope())
//            {

//                var searchEThcDEngine = scope.Resolve<ISearchEngine>();
//                searchEThcDEngine.Init(
//                    @"C:\Users\iruiz\Desktop\app\ZC_20171218_H95_R1.raw",
//                    @"C:\Users\iruiz\Desktop\app\HP.fasta",
//                    @"C:\Users\iruiz\Desktop\app\test.csv");

//                progress sender = new progress(printScan);
//                searchEThcDEngine.Search(searchEThcDEngine.GetFirstScan(), 300, sender);
//                searchEThcDEngine.Analyze(searchEThcDEngine.GetFirstScan(), 300);

//            }

//            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
//            Console.Read();

//        }
//        public void printScan(int scan)
//        {
//            Console.WriteLine(scan);
//        }
//    }
//}
