//using GlycoSeqClassLibrary.Algorithm;
//using GlycoSeqClassLibrary.Analyze;
//using GlycoSeqClassLibrary.Analyze.Reporter;
//using GlycoSeqClassLibrary.Analyze.Result;
//using GlycoSeqClassLibrary.Analyze.Score;
//using GlycoSeqClassLibrary.Builder.Chemistry.Glycan;
//using GlycoSeqClassLibrary.Builder.Chemistry.Glycan.Mass;
//using GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide;
//using GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide.Mass;
//using GlycoSeqClassLibrary.Builder.Chemistry.Peptide;
//using GlycoSeqClassLibrary.Builder.Chemistry.Peptide.Generator;
//using GlycoSeqClassLibrary.Builder.Chemistry.Peptide.Parameter;
//using GlycoSeqClassLibrary.Builder.Chemistry.Protein;
//using GlycoSeqClassLibrary.Builder.Chemistry.Protein.Fasta;
//using GlycoSeqClassLibrary.Builder.Spectrum;
//using GlycoSeqClassLibrary.Builder.Spectrum.ThermoRaw;
//using GlycoSeqClassLibrary.Model.Chemistry.Glycan;
//using GlycoSeqClassLibrary.Model.Chemistry.Glycan.TableNGlycan;
//using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
//using GlycoSeqClassLibrary.Model.Chemistry.Peptide;
//using GlycoSeqClassLibrary.Model.Chemistry.Protein;
//using GlycoSeqClassLibrary.Model.Spectrum;
//using GlycoSeqClassLibrary.Search.Precursor;
//using GlycoSeqClassLibrary.Search.Process;
//using GlycoSeqClassLibrary.Search.Process.MonoMass;
//using GlycoSeqClassLibrary.Search.Process.Normalize;
//using GlycoSeqClassLibrary.Search.Process.PeakPicking;
//using GlycoSeqClassLibrary.Search.Process.PeakPicking.PeakPickingDelegator;
//using GlycoSeqClassLibrary.Search.SearchEThcD;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ConsoleAppTest
//{
//    public class TestCase7 :TestCase
//    {
//        public void Run()
//        {
//            var watch = new System.Diagnostics.Stopwatch();
//            watch.Start();
//            // protein
//            IProteinDataBuilder proteinBuilder = new GeneralFastaDataBuilder();
//            IProteinCreator proteinCreator = new GeneralProteinCreator(proteinBuilder);
//            List<IProtein> proteins = proteinCreator.Create(@"C:\Users\iruiz\Desktop\app\HP.fasta");
//            // peptides
//            List<IPeptideSequencesGenerator> generatorList = new List<IPeptideSequencesGenerator>();
//            IPeptideSequencesGeneratorParameter parameter = new GeneralPeptideGeneratorParameter();
//            parameter.SetProtease(Proteases.GluC);
//            NGlycosylatedPeptideSequencesGenerator generatorGluc = new NGlycosylatedPeptideSequencesGenerator(parameter);
//            generatorList.Add(generatorGluc);
//            parameter = new GeneralPeptideGeneratorParameter();
//            parameter.SetProtease(Proteases.Trypsin);
//            NGlycosylatedPeptideSequencesGenerator generatorTrypsin = new NGlycosylatedPeptideSequencesGenerator(parameter);
//            generatorList.Add(generatorTrypsin);
//            IPeptideSequencesGenerator peptideSequencesGenerator = new DoubleDigestionPeptideSequencesGeneratorProxy(generatorList);
//            IPeptideCreator peptideCreator = new GeneralPeptideCreator(peptideSequencesGenerator);
//            List<IPeptide> peptides = new List<IPeptide>();
//            HashSet<string> seen = new HashSet<string>();
//            foreach (IProtein protein in proteins)
//            {
//                foreach (IPeptide peptide in peptideCreator.Create(protein))
//                {
//                    if (!seen.Contains(peptide.GetSequence()))
//                    {
//                        seen.Add(peptide.GetSequence());
//                        peptides.Add(peptide);
//                    }
//                }
//            }
//            // glycans
//            ITableNGlycanProxyGenerator tableNGlycanProxyGenerator = new GeneralTableNGlycanMassProxyGenerator(12, 12, 5, 4, 0);
//            int[] structTable = new int[24];
//            structTable[0] = 1;
//            ITableNGlycanProxy root = new GeneralTableNGlycanMassProxy(new ComplexNGlycan(structTable));
//            IGlycanCreator glycanCreator = new GeneralTableNGlycanCreator(tableNGlycanProxyGenerator, root);
//            List<IGlycan> glycans = glycanCreator.Create();



//            // precursor
//            List<IPoint> points = new List<IPoint>();
//            foreach (IGlycan glycan in glycans)
//            {
//                points.Add(new GlycanPoint(glycan));
//            }

//            IComparer<IPoint> comparer = new ToleranceComparer(0.01);
//            //ISearch matcher = new BinarySearch(points, comparer);
//            ISearch matcher = new BucketSearch(points, comparer, 0.01);
//            IGlycoPeptideProxyGenerator glycoPeptideProxyGenerator = new GeneralTableNGlycoPeptideMassProxyGenerator();
//            IGlycoPeptideCreator glycoPeptideCreator = new GeneralNGlycoPeptideSingleSiteCreator(glycoPeptideProxyGenerator);
//            IPrecursorMatcher precursorMatcher = new GeneralPrecursorMatcher(peptides, matcher, glycoPeptideCreator);

//            // spectrum
//            ISpectrumReader spectrumReader = new ThermoRawSpectrumReader();
//            spectrumReader.Init(@"C:\Users\iruiz\Desktop\app\ZC_20171218_H95_R1.raw");
//            ISpectrumFactory spectrumFactory = new GeneralSpectrumFactory(spectrumReader);

            
//            IComparer<IPoint> comparer2 = new ToleranceComparer(0.01); //new PPMComparer(20);
//            //ISearch matcherPeaks = new BinarySearch(points, comparer2);
//            ISearch matcherPeaks = new BucketSearch(points, comparer2, 0.01);
//            //ISearch matcherPeaks = new BucketSearch(points, 0.01);
//            IScoreFactory scoreFactory = new GeneralScoreFactory(1.0, 0.0);
//            IGlycoPeptidePointsCreator glycoPeptidePointsCreator = new GeneralGlycoPeptideMassProxyPointsCreator();
//            ISearchEThcD searchEThcDRunner = new GeneralSearchEThcD(matcherPeaks, scoreFactory, glycoPeptidePointsCreator);

//            IPeakPickingDelegator delegator = new TopIntensityPeakPickingDelegator(100);
//            ISpectrumProcessing spectrumProcessing = new GeneralPeakPickingSpectrumProcessing(delegator);
//            ISpectrumProcessing normalizer = new GeneralNormalizeSpectrumProcessing(1.0);
//            IResults results = new GeneralResults();

//            IComparer<IPoint> comparer3 = new PPMComparer(5);
//            ISearch matcherSpectrum = new BinarySearch(new List<IPoint>(), comparer3);
//            IMonoMassSpectrumGetter monoMassSpectrumGetter = new GeneralMonoMassSpectrumGetter(matcherSpectrum);

//            for (var i = 0; i < spectrumFactory.GetLastScan(); i++)
//            {
//                ISpectrum spectrum = spectrumFactory.GetSpectrum(i);
//                if (spectrum.GetMSnOrder() < 2)
//                {
//                    monoMassSpectrumGetter.SetMonoMassSpectrum(spectrum);
//                    continue;
//                }

//                double monoMass = monoMassSpectrumGetter.GetMonoMass(spectrum as ISpectrumMSn);
               
//                normalizer.Process(spectrum);
//                spectrumProcessing.Process(spectrum);
//                List<IGlycoPeptide> glycoPeptides = precursorMatcher.Match(spectrum, monoMass);

//                // search
//                List<IScore> scores = new List<IScore>();
//                double maxScores = 0;
//                foreach (IGlycoPeptide glycoPeptide in glycoPeptides)
//                {
//                    IScore score = searchEThcDRunner.Search(spectrum, glycoPeptide);
//                    scores.Add(score);
//                    maxScores = Math.Max(maxScores, score.GetScore());
//                }
//                if (scores.Count > 0)
//                    results.Add(spectrum, scores.Where(x => x.GetScore() == maxScores).ToList());
//            }
//            //ISpectrum spectrum = spectrumFactory.GetSpectrum(7039);

//            watch.Stop();
//            IReportProducer reportProducer = new CSVReportProducer();
//            reportProducer.SetOutputLocation(@"C:\Users\iruiz\Desktop\app\test.csv");
//            reportProducer.Report(results, 0, spectrumFactory.GetLastScan());

//            //Console.WriteLine(maxScores);

//            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
//            Console.Read();

//        }
//    }
//}
