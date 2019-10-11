using GlycoSeqClassLibrary.Algorithm;
using GlycoSeqClassLibrary.Analyze;
using GlycoSeqClassLibrary.Analyze.Reporter;
using GlycoSeqClassLibrary.Analyze.Result;
using GlycoSeqClassLibrary.Analyze.Score;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycan;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycan.Mass;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide.Mass;
using GlycoSeqClassLibrary.Builder.Chemistry.Peptide;
using GlycoSeqClassLibrary.Builder.Chemistry.Peptide.Generator;
using GlycoSeqClassLibrary.Builder.Chemistry.Peptide.Parameter;
using GlycoSeqClassLibrary.Builder.Chemistry.Protein;
using GlycoSeqClassLibrary.Builder.Chemistry.Protein.Fasta;
using GlycoSeqClassLibrary.Builder.Spectrum;
using GlycoSeqClassLibrary.Builder.Spectrum.ThermoRaw;
using GlycoSeqClassLibrary.Engine;
using GlycoSeqClassLibrary.Engine.SearchEThcD;
using GlycoSeqClassLibrary.Model.Chemistry.Glycan;
using GlycoSeqClassLibrary.Model.Chemistry.Glycan.TableNGlycan;
using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using GlycoSeqClassLibrary.Model.Chemistry.Peptide;
using GlycoSeqClassLibrary.Model.Chemistry.Protein;
using GlycoSeqClassLibrary.Model.Spectrum;
using GlycoSeqClassLibrary.Search.Precursor;
using GlycoSeqClassLibrary.Search.Process;
using GlycoSeqClassLibrary.Search.Process.MonoMass;
using GlycoSeqClassLibrary.Search.Process.Normalize;
using GlycoSeqClassLibrary.Search.Process.PeakPicking;
using GlycoSeqClassLibrary.Search.Process.PeakPicking.PeakPickingDelegator;
using GlycoSeqClassLibrary.Search.SearchEThcD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    public class TestCase8 : TestCase
    {
        public void Run()
        {

            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            // protein
            IProteinDataBuilder proteinBuilder = new GeneralFastaDataBuilder();
            IProteinCreator proteinCreator = new GeneralProteinCreator(proteinBuilder);
            
            // peptides
            List<IPeptideSequencesGenerator> generatorList = new List<IPeptideSequencesGenerator>();
            IPeptideSequencesGeneratorParameter parameter = new GeneralPeptideGeneratorParameter();
            parameter.SetProtease(Proteases.GluC);
            NGlycosylatedPeptideSequencesGenerator generatorGluc = new NGlycosylatedPeptideSequencesGenerator(parameter);
            generatorList.Add(generatorGluc);
            parameter = new GeneralPeptideGeneratorParameter();
            parameter.SetProtease(Proteases.Trypsin);
            NGlycosylatedPeptideSequencesGenerator generatorTrypsin = new NGlycosylatedPeptideSequencesGenerator(parameter);
            generatorList.Add(generatorTrypsin);
            IPeptideSequencesGenerator peptideSequencesGenerator = new DoubleDigestionPeptideSequencesGeneratorProxy(generatorList);
            IPeptideCreator peptideCreator = new GeneralPeptideCreator(peptideSequencesGenerator);

            
            // glycans
            ITableNGlycanProxyGenerator tableNGlycanProxyGenerator = new GeneralTableNGlycanMassProxyGenerator(12, 12, 5, 4, 0);
            int[] structTable = new int[24];
            structTable[0] = 1;
            ITableNGlycanProxy root = new GeneralTableNGlycanMassProxy(new ComplexNGlycan(structTable));
            IGlycanCreator glycanCreator = new GeneralTableNGlycanCreator(tableNGlycanProxyGenerator, root);
            
            // precursor
            
            IComparer<IPoint> comparer = new ToleranceComparer(0.01);
            ISearch matcher = new BucketSearch(comparer, 0.01);
            IGlycoPeptideProxyGenerator glycoPeptideProxyGenerator = new GeneralTableNGlycoPeptideMassProxyGenerator();
            IGlycoPeptideCreator glycoPeptideCreator = new GeneralNGlycoPeptideSingleSiteCreator(glycoPeptideProxyGenerator);
            IPrecursorMatcher precursorMatcher = new GeneralPrecursorMatcher(matcher, glycoPeptideCreator);

            // spectrum
            ISpectrumReader spectrumReader = new ThermoRawSpectrumReader();
            ISpectrumFactory spectrumFactory = new GeneralSpectrumFactory(spectrumReader);


            IComparer<IPoint> comparer2 = new PPMComparer(20);
            ISearch matcherPeaks = new BucketSearch(comparer2, 0.01);
            IScoreFactory scoreFactory = new GeneralScoreFactory(1.0, 0.0);
            IGlycoPeptidePointsCreator glycoPeptidePointsCreator = new GeneralGlycoPeptideMassProxyPointsCreator();
            ISearchEThcD searchEThcDRunner = new GeneralSearchEThcD(matcherPeaks, scoreFactory, glycoPeptidePointsCreator);

            IPeakPickingDelegator delegator = new TopIntensityPeakPickingDelegator(100);
            ISpectrumProcessing peakPicking = new GeneralPeakPickingSpectrumProcessing(delegator);
            ISpectrumProcessing normalizer = new GeneralNormalizeSpectrumProcessing(1.0);
            ISpectrumProcessingProxy spectrumProcessing = new SpectrumPRocessingProxy();
            spectrumProcessing.Add(peakPicking);
            spectrumProcessing.Add(normalizer);
            IResults results = new GeneralResults();

            IComparer<IPoint> comparer3 = new PPMComparer(5);
            ISearch matcherSpectrum = new BinarySearch(comparer3);
            IMonoMassSpectrumGetter monoMassSpectrumGetter = new GeneralMonoMassSpectrumGetter(matcherSpectrum);

            IReportProducer reportProducer = new CSVReportProducer();

            ISearchEngine searchEThcDEngine = new GeneralSearchEThcDEngine(proteinCreator, peptideCreator, glycanCreator, 
                spectrumReader, spectrumFactory, spectrumProcessing, monoMassSpectrumGetter, precursorMatcher, searchEThcDRunner,
                results, reportProducer);

            searchEThcDEngine.Init(
                @"C:\Users\iruiz\Desktop\app\ZC_20171218_H95_R1.raw",
                @"C:\Users\iruiz\Desktop\app\HP.fasta",
                @"C:\Users\iruiz\Desktop\app\test.csv");

            for(int scan = searchEThcDEngine.GetFirstScan(); scan <= searchEThcDEngine.GetLastScan(); scan++)
            {
                searchEThcDEngine.Search(scan);
            }

            searchEThcDEngine.Analyze(searchEThcDEngine.GetFirstScan(), searchEThcDEngine.GetLastScan());
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
            Console.Read();
        }
    }
}
