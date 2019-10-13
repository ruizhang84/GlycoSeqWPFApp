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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GlycoSeqWPFApp
{
    /// <summary>
    /// SearchWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SearchWindow : Window
    {
        Autofac.IContainer container;
        ILifetimeScope scope;
        ISearchEngine searchEngine;

        public int StartScan { get; set; }
        public int EndScan { get; set; }
        public int MinScan { get; set; }
        public int MaxScan { get; set; }

        public SearchWindow()
        {
            InitializeComponent();
            InitializeContainer();
            InitializeSearchEngine();
        }

        ~SearchWindow()
        {
            scope.Dispose();
        }

        private void InitializeSearchEngine()
        {
            scope = container.BeginLifetimeScope();
            searchEngine = scope.Resolve<ISearchEngine>();
            searchEngine.Init(SearchParameters.Access.MSMSFile, 
                            SearchParameters.Access.FastaFile, 
                            SearchParameters.Access.OutputFile);
            StartScan = MinScan = searchEngine.GetFirstScan();
            EndScan = MaxScan = searchEngine.GetLastScan();
            start.Text = StartScan.ToString();
            end.Text = EndScan.ToString();
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
            { Enzymes = SearchParameters.Access.DigestionEnzyme,
              MiniLength = SearchParameters.Access.MiniPeptideLength,
              MissCleavage = SearchParameters.Access.MissCleavage
            });

            builder.RegisterModule(new FastaProteinModule());
            builder.RegisterModule(new FDRCSVReportModule() { FDR = SearchParameters.Access.FDRValue });

            builder.RegisterModule(new MonoMassSpectrumGetterModule()
            {
                Tolerance = SearchParameters.Access.PrecursorTolerance,
                MaxIsotop = SearchParameters.Access.MaxIsotopic,
                ScanRange = SearchParameters.Access.ScanRange
            });
            builder.RegisterModule(new PrecursorMatcherModule() { Tolerance = SearchParameters.Access.MS1Tolerance });
            builder.RegisterModule(new SearchEThcDModule()
            {
                Tolerance = SearchParameters.Access.MSMSTolerance,
                alpha = SearchParameters.Access.Alpah, beta = SearchParameters.Access.Beta,
                glycanWeight = SearchParameters.Access.GlycanWeight, peptideWeight = SearchParameters.Access.PeptideWeight
            });

            builder.RegisterModule(new TopPeakPickingDelegatorModule() { MaxPeaks = SearchParameters.Access.MaxPeaksNum });
            builder.RegisterModule(new SpectrumProcessingModule() { ScaleFactor = SearchParameters.Access.ScaleFactor });
            builder.RegisterModule(new ThermoRawSpectrumModule());

            builder.Register(c => new FDRSearchEThcDEngine(c.Resolve<IProteinCreator>(), c.Resolve<IPeptideCreator>(),
                c.Resolve<IGlycanCreator>(), c.Resolve<ISpectrumReader>(), c.Resolve<ISpectrumFactory>(), c.Resolve<ISpectrumProcessing>(),
                c.Resolve<IMonoMassSpectrumGetter>(), c.Resolve<IPrecursorMatcher>(), c.Resolve<ISearchEThcD>(),
                c.Resolve<IResults>(), c.Resolve<IReportProducer>())).As<ISearchEngine>();

           container  = builder.Build();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkCompleted;
            signal.Text = "Searching...";
            worker.RunWorkerAsync();
            buttonRun.IsEnabled = false;
            start.IsEnabled = false;
            end.IsEnabled = false;
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            searchEngine.Init(SearchParameters.Access.MSMSFile, SearchParameters.Access.FastaFile, SearchParameters.Access.OutputFile);
            for (int i = StartScan; i <= EndScan; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                searchEngine.Search(i);
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgessStatus.Text = e.ProgressPercentage.ToString();
            SearchingStatus.Value = Math.Max(SearchingStatus.Value,
                (e.ProgressPercentage - StartScan) * 1.0 / (EndScan - StartScan) * 1000.0);
        }

        void worker_RunWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            searchEngine.Analyze(StartScan, EndScan);
            signal.Text = "Done";
        }

        private void Start_TextChanged(object sender, TextChangedEventArgs e)
        {
            int value = -1;
            if (Int32.TryParse(start.Text, out value))
            {
                if (value <= MaxScan && value >= MinScan)
                {
                    StartScan = value;
                }
                else
                {
                    start.Text = MinScan.ToString();
                }
            }
            else
            {
                start.Text = StartScan.ToString();
            }

        }

        private void End_TextChanged(object sender, TextChangedEventArgs e)
        {
            int value = -1;
            if (int.TryParse(end.Text, out value))
            {
                if (value <= MaxScan && value >= MinScan)
                {
                    EndScan = value;
                }
                else
                {
                    end.Text = MaxScan.ToString();
                }

            }
            else
            {
                end.Text = EndScan.ToString();
            }
        }


    }
}
