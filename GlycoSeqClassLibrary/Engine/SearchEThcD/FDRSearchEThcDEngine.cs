﻿using GlycoSeqClassLibrary.Analyze;
using GlycoSeqClassLibrary.Analyze.Score;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycan;
using GlycoSeqClassLibrary.Builder.Chemistry.Peptide;
using GlycoSeqClassLibrary.Builder.Chemistry.Protein;
using GlycoSeqClassLibrary.Builder.Spectrum;
using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using GlycoSeqClassLibrary.Model.Spectrum;
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
            ISpectrumReader spectrumReader,
            ISpectrumFactory spectrumFactory,
            ISpectrumProcessing spectrumProcessing,
            IMonoMassSpectrumGetter monoMassSpectrumGetter,
            IPrecursorMatcher precursorMatcher,
            ISearchEThcD searchEThcDRunner,
            IResults results,
            IReportProducer reportProducer,
            double pesudoMass = 50.0):
            base(proteinCreator, peptideCreator, glycanCreator, spectrumReader, spectrumFactory,
                spectrumProcessing, monoMassSpectrumGetter, precursorMatcher, searchEThcDRunner, results, reportProducer)
        {
            this.pesudoMass = pesudoMass;
        }

        public override void Search(int scan)
        {
            ISpectrum spectrum = spectrumFactory.GetSpectrum(scan);
            if (spectrum.GetMSnOrder() < 2)
            {
                monoMassSpectrumGetter.SetMonoMassSpectrum(spectrum);
                return;
            }

            // precursor
            double monoMass = monoMassSpectrumGetter.GetMonoMass(spectrum as ISpectrumMSn);
            spectrumProcessing.Process(spectrum);

            List<IGlycoPeptide> glycoPeptides = precursorMatcher.Match(spectrum, monoMass);
            List<IGlycoPeptide> decoyGlycoPeptides = precursorMatcher.Match(spectrum, monoMass + pesudoMass);

            // search
            List<IScore> scores = new List<IScore>();
            foreach (IGlycoPeptide glycoPeptide in glycoPeptides)
            {
                IScore score = searchEThcDRunner.Search(spectrum, glycoPeptide);
                IScoreProxy scoreProxy = new FDRScoreProxy(score, true);
                scores.Add(scoreProxy);
            }

            List<IScore> decoyScores = new List<IScore>();
            foreach (IGlycoPeptide decoyGlycoPeptide in decoyGlycoPeptides)
            {
                IScore score = searchEThcDRunner.Search(spectrum, decoyGlycoPeptide);
                IScoreProxy scoreProxy = new FDRScoreProxy(score, false);
                decoyScores.Add(scoreProxy);
            }

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