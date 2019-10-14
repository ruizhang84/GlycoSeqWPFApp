using GlycoSeqClassLibrary.Algorithm;
using GlycoSeqClassLibrary.Analyze;
using GlycoSeqClassLibrary.Analyze.Result;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycan;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide;
using GlycoSeqClassLibrary.Builder.Chemistry.Peptide;
using GlycoSeqClassLibrary.Builder.Chemistry.Protein;
using GlycoSeqClassLibrary.Builder.Spectrum;
using GlycoSeqClassLibrary.Model.Chemistry.Glycan;
using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using GlycoSeqClassLibrary.Model.Chemistry.Peptide;
using GlycoSeqClassLibrary.Model.Chemistry.Protein;
using GlycoSeqClassLibrary.Model.Spectrum;
using GlycoSeqClassLibrary.Search.Precursor;
using GlycoSeqClassLibrary.Search.Process;
using GlycoSeqClassLibrary.Search.Process.PeakPicking;
using GlycoSeqClassLibrary.Search.SearchEThcD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Engine.SearchEThcD
{
    public class GeneralSearchEThcDEngine : ISearchEngine
    {
        protected IProteinCreator proteinCreator;
        protected List<IProtein> proteins;

        protected IPeptideCreator peptideCreator;
        protected List<IPeptide> peptides;

        protected IGlycanCreator glycanCreator;
        protected List<IGlycan> glycans;

        protected ISpectrumFactory spectrumFactory;

        protected ISpectrumProcessing spectrumProcessing;
        protected IMonoMassSpectrumGetter monoMassSpectrumGetter;

        protected IPrecursorMatcher precursorMatcher;

        protected ISearchEThcD searchEThcDRunner;

        protected IResults results;
        protected IReportProducer reportProducer;       

        public GeneralSearchEThcDEngine(
            IProteinCreator proteinCreator,
            IPeptideCreator peptideCreator,
            IGlycanCreator glycanCreator,
            ISpectrumFactory spectrumFactory,
            ISpectrumProcessing spectrumProcessing,
            IMonoMassSpectrumGetter monoMassSpectrumGetter,
            IPrecursorMatcher precursorMatcher,
            ISearchEThcD searchEThcDRunner,
            IResults results,
            IReportProducer reportProducer
            )
        {
            // protein
            this.proteinCreator = proteinCreator;

            // peptides
            this.peptideCreator = peptideCreator;
            
            // glycans
            this.glycanCreator = glycanCreator;

            // spectrum
            this.spectrumFactory = spectrumFactory;

            // spectrum processing
            this.spectrumProcessing = spectrumProcessing;
            this.monoMassSpectrumGetter = monoMassSpectrumGetter;

            // glycopeptides
            this.precursorMatcher = precursorMatcher;

            // search
            this.searchEThcDRunner = searchEThcDRunner;

            // result
            this.results = results;
            this.reportProducer = reportProducer;
        }

        public int GetFirstScan()
        {
            return spectrumFactory.GetFirstScan();
        }

        public int GetLastScan()
        {
            return spectrumFactory.GetLastScan();
        }

        public void Init(string spectrumFileLocation, string peptideFileLocation, string outputLocation)
        {
            spectrumFactory.Init(spectrumFileLocation);
            proteins = proteinCreator.Create(peptideFileLocation);
            HashSet<string> seen = new HashSet<string>();
            peptides = new List<IPeptide>();
            foreach (IProtein protein in proteins)
            {
                foreach (IPeptide peptide in peptideCreator.Create(protein))
                {
                    if (!seen.Contains(peptide.GetSequence()))
                    {
                        seen.Add(peptide.GetSequence());
                        peptides.Add(peptide);
                    }
                }
            }
            glycans = glycanCreator.Create();
            precursorMatcher.SetPeptides(peptides);
            precursorMatcher.SetGlycans(glycans);
            reportProducer.SetOutputLocation(outputLocation);
        }

        public virtual void Search(int scan)
        {
            ISpectrum spectrum = spectrumFactory.GetSpectrum(scan);
            double monoMass = monoMassSpectrumGetter.GetMonoMass(spectrum);     //assume read spectrum sequentially
            if (spectrum.GetMSnOrder() < 2)
            {
                return;
            }
            
            // precursor
            spectrumProcessing.Process(spectrum);
            List<IGlycoPeptide> glycoPeptides = precursorMatcher.Match(spectrum, monoMass);

            // search
            List<IScore> scores = new List<IScore>();
            foreach (IGlycoPeptide glycoPeptide in glycoPeptides)
            {
                IScore score = searchEThcDRunner.Search(spectrum, glycoPeptide);
                scores.Add(score);
            }

            // save results
            if (scores.Count > 0)
            {
                double maxScores = scores.Max(x => x.GetScore());
                results.Add(spectrum, scores.Where(x => x.GetScore() == maxScores).ToList());
            }               
        }

        public void Analyze(int start, int end)
        {
           Analyze(start, end, results); 
        }

        public void Analyze(int start, int end, IResults results)
        {
            reportProducer.Report(results, start, end);
        }

        public void Search(int start, int end, progress sender)
        {
            for (int i = start; i <= end; i++)
            {
                Search(i);
                sender(i);
            }
        }

        public IResults GetResults()
        {
            return results;
        }
    }
}
