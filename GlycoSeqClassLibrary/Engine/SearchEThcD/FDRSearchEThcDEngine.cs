using GlycoSeqClassLibrary.Analyze;
using GlycoSeqClassLibrary.Analyze.Score;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycan;
using GlycoSeqClassLibrary.Builder.Chemistry.Peptide;
using GlycoSeqClassLibrary.Builder.Chemistry.Protein;
using GlycoSeqClassLibrary.Builder.Spectrum;
using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using GlycoSeqClassLibrary.Model.Spectrum;
using GlycoSeqClassLibrary.Search.Filter;
using GlycoSeqClassLibrary.Search.Precursor;
using GlycoSeqClassLibrary.Search.Process;
using GlycoSeqClassLibrary.Search.SearchEThcD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Engine.SearchEThcD
{
    public class FDRSearchEThcDEngine : GeneralSearchEThcDEngine
    {
        double pesudoMass;

        public FDRSearchEThcDEngine(IProteinCreator proteinCreator,
            IPeptideCreator peptideCreator,
            IGlycanCreator glycanCreator,
            ISpectrumFactory spectrumFactory,
            ISpectrumProcessing spectrumProcessing,
            IMonoMassSpectrumGetter monoMassSpectrumGetter,
            IPrecursorMatcher precursorMatcher,
            ISpectrumFilter spectrumFilter,
            ISearchEThcD searchEThcDRunner,
            IResults results,
            IReportProducer reportProducer,
            double pesudoMass = 40.0):
            base(proteinCreator, peptideCreator, glycanCreator, spectrumFactory,
                spectrumProcessing, monoMassSpectrumGetter, precursorMatcher, spectrumFilter, searchEThcDRunner, results, reportProducer)
        {
            this.pesudoMass = pesudoMass;
        }

        private string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public override void Search(int scan)
        {
            ISpectrum spectrum = spectrumFactory.GetSpectrum(scan);
            double monoMass = monoMassSpectrumGetter.GetMonoMass(spectrum); // assume read spectrum sequentially.
            if (spectrum.GetMSnOrder() < 2)
            {
                return;
            } 

            // ISpectrum filter
            if (spectrumFilter.Filter(spectrum)) return;

            // precursor
            spectrumProcessing.Process(spectrum);
            List<IGlycoPeptide> glycoPeptides = precursorMatcher.Match(spectrum, monoMass);
            double charge = (spectrum as ISpectrumMSn).GetParentCharge();
            List<IGlycoPeptide> decoyGlycoPeptides = precursorMatcher.Match(spectrum, monoMass + pesudoMass / charge);

            // search
            List<IScore> scores = new List<IScore>();
            foreach (IGlycoPeptide glycoPeptide in glycoPeptides)
            {
                IScore score = searchEThcDRunner.Search(spectrum, glycoPeptide);
                IScoreProxy scoreProxy = new FDRScoreProxy(score, true);
                scores.Add(scoreProxy);
                //Console.WriteLine();
            }

            //Console.WriteLine("Decoy:");
            List<IScore> decoyScores = new List<IScore>();
            foreach (IGlycoPeptide decoyGlycoPeptide in decoyGlycoPeptides)
            {
                decoyGlycoPeptide.SetPeptide(decoyGlycoPeptide.GetPeptide().Clone());
                decoyGlycoPeptide.GetPeptide().SetSequence(Reverse(decoyGlycoPeptide.GetPeptide().GetSequence()));
                IScore score = searchEThcDRunner.Search(spectrum, decoyGlycoPeptide);
                IScoreProxy scoreProxy = new FDRScoreProxy(score, false);
                decoyScores.Add(scoreProxy);
            }
            //Console.WriteLine();
            //Console.WriteLine();

            // save results
            List<IScore> finalScore = new List<IScore>();
            if (scores.Count > 0)
            {
                double maxScores = scores.Max(x => x.GetScore());
                finalScore.AddRange(scores.Where(x => x.GetScore() == maxScores).ToList());
            }

            if (decoyScores.Count > 0)
            {
                double maxScores = decoyScores.Max(x => x.GetScore());
                finalScore.AddRange(decoyScores.Where(x => x.GetScore() == maxScores).ToList());
               
            }
            if (finalScore.Count > 0)
            {
                results.Add(spectrum, finalScore);
            }
            
        }
    }
}
