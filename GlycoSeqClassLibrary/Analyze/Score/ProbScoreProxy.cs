using GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide.Mass;
using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using GlycoSeqClassLibrary.Model.Spectrum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Analyze.Score
{
    public class ProbScoreProxy : IProbScoreProxy
    {
        IScore score;
        double probability;
        ISpectrum spectrum;

        public ProbScoreProxy(IScore score, ISpectrum spectrum, double probability)
        {
            this.score = score;
            this.spectrum = spectrum;
            this.probability = probability;
        }

        public double GetProbability()
        {
            return probability;
        }

        public bool IsDecoy()
        {
            return (score as IFDRScoreProxy).IsDecoy();
        }

        public void AddScore(IPeak peak)
        {
            score.AddScore(peak);
        }
        public void AddCoreScore(IPeak peak)
        {
            score.AddCoreScore(peak);
        }

        public void AddBranchScore(IPeak peak)
        {
            score.AddBranchScore(peak);
        }

        public void AddPeptideScore(IPeak peak)
        {
            score.AddPeptideScore(peak);
        }

        public IGlycoPeptide GetGlycoPeptide()
        {
            return score.GetGlycoPeptide();
        }

        public double GetScore()
        {
            return score.GetScore();
        }

        public void AddScore(IScore other)
        {
            score.AddScore(other);
        }

        public double GetScore(MassType type)
        {
            return score.GetScore(type);
        }

        public ISpectrum GetSpectrum()
        {
            return spectrum;
        }
    }
}
