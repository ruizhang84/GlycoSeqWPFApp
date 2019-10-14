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
using GlycoSeqClassLibrary.Search.Precursor;
using GlycoSeqClassLibrary.Search.Process;
using GlycoSeqClassLibrary.Search.SearchEThcD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GlycoSeqWPFApp
{
    /// <summary>
    /// SearchWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SearchWindow : Window
    {
        Autofac.IContainer container;
        int progressCounter;
        IResults Results { get; set; }

        public int StartScan { get; set; }
        public int EndScan { get; set; }

        public SearchWindow()
        {
            InitializeComponent();
            InitializeContainer();
            InitializeWindow();
            ContentRendered += WindowLoaded;  
        } 

        private void InitializeWindow()
        {
            using (var scope = container.BeginLifetimeScope())
            {
                ISpectrumFactory spectrumFactory = scope.Resolve<ISpectrumFactory>();
                Results = scope.Resolve<IResults>();
                spectrumFactory.Init(SearchParameters.Access.MSMSFile);
                StartScan  = spectrumFactory.GetFirstScan();
                EndScan = spectrumFactory.GetLastScan();
            }        
        }

        private void InitializeContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new NGlycanModule()
            {
                HexNAcBound = SearchParameters.Access.HexNAc,
                HexBound = SearchParameters.Access.Hex,
                FucBound = SearchParameters.Access.Fuc,
                NeuAcBound = SearchParameters.Access.NeuAc,
                NeuGcBound = SearchParameters.Access.NeuGc
            });
            builder.RegisterModule(new NGlycoPeptideModule());

            builder.RegisterModule(new DoubleDigestionPeptidesModule()
            {
                Enzymes = SearchParameters.Access.DigestionEnzyme,
                MiniLength = SearchParameters.Access.MiniPeptideLength,
                MissCleavage = SearchParameters.Access.MissCleavage
            });

            builder.RegisterModule(new FastaProteinModule());
            builder.RegisterModule(new FDRCSVReportModule() { FDR = SearchParameters.Access.FDRValue });

            builder.RegisterModule(new MonoMassSpectrumGetterModule()
            {
                Tolerance = SearchParameters.Access.PrecursorTolerance,
                MaxIsotop = SearchParameters.Access.MaxIsotopic
            });
            builder.RegisterModule(new PrecursorMatcherModule() { Tolerance = SearchParameters.Access.MS1Tolerance });
            builder.RegisterModule(new SearchEThcDModule()
            {
                Tolerance = SearchParameters.Access.MSMSTolerance,
                alpha = SearchParameters.Access.Alpah,
                beta = SearchParameters.Access.Beta,
                glycanWeight = SearchParameters.Access.GlycanWeight,
                peptideWeight = SearchParameters.Access.PeptideWeight
            });

            builder.RegisterModule(new TopPeakPickingDelegatorModule() { MaxPeaks = SearchParameters.Access.MaxPeaksNum });
            builder.RegisterModule(new SpectrumProcessingModule() { ScaleFactor = SearchParameters.Access.ScaleFactor });
            builder.RegisterModule(new ThermoRawSpectrumModule());

            builder.Register(c => new FDRSearchEThcDEngine(c.Resolve<IProteinCreator>(), c.Resolve<IPeptideCreator>(),
                c.Resolve<IGlycanCreator>(), c.Resolve<ISpectrumFactory>(), c.Resolve<ISpectrumProcessing>(),
                c.Resolve<IMonoMassSpectrumGetter>(), c.Resolve<IPrecursorMatcher>(), c.Resolve<ISearchEThcD>(),
                c.Resolve<IResults>(), c.Resolve<IReportProducer>())).As<ISearchEngine>();

            container = builder.Build();
        }

        private void WindowLoaded(object sender, EventArgs e)
        {

            Task.Run(Process);
        } 

        private Task Process()
        {
            progressCounter = 0;
            Counter counter = new Counter();
            counter.progressChange += SearchProgressChanged;

            UpdateSignal("Searching...");
            MultiThreadSearch search = new MultiThreadSearch(counter, container, Results);
            search.Run();

            UpdateSignal("Analyzing...");
            Analyze();

            UpdateSignal("Done");
            return Task.CompletedTask;
        }

        private void Analyze()
        {
            using (var scope = container.BeginLifetimeScope())
            {
                ISearchEngine searchEngine = scope.Resolve<ISearchEngine>();
                searchEngine.Init(SearchParameters.Access.MSMSFile,
                                SearchParameters.Access.FastaFile,
                                SearchParameters.Access.OutputFile);
                searchEngine.Analyze(StartScan, EndScan, Results);
            }
        }

        private void UpdateSignal(string signal)
        {
            Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                new ThreadStart(() => Signal.Text = signal));
        }

        private void UpdateProgress()
        {
            SearchingStatus.Value = progressCounter * 1.0 / (EndScan - StartScan) * 1000.0;
            ProgessStatus.Text = progressCounter.ToString();
        }

        private void SearchProgressChanged(object sender, EventArgs e)
        {
            Interlocked.Increment(ref progressCounter); 
            Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                new ThreadStart(UpdateProgress));
        } 
    }

    public class Counter
    {
        public event EventHandler progressChange;

        protected virtual void OnProgressChanged(EventArgs e)
        {
            EventHandler handler = progressChange;
            handler?.Invoke(this, e);
        }

        public void Add(int scan)
        {
            EventArgs e = new EventArgs();
            OnProgressChanged(e);
        }
    }

}
