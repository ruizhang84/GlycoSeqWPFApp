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
    public class WeightedScore : IScore
    {
        IGlycoPeptide glycoPeptide;
        HashSet<double> peakInclude;

        double score;
        double alpha;
        double beta;
        double glycanWeight;
        double coreGlycanWeight;
        double peptideWeight;

        int glycanNum;
        int coreGlycanNum;
        int peptideNum;

        public WeightedScore(IGlycoPeptide glycoPeptide, double alpha, double beta,
            double glycanWeight, double coreGlycanWeight, double peptideWeight)
        {
            this.score = 0.0;
            this.glycoPeptide = glycoPeptide;

            this.alpha = alpha * 100;
            this.beta = beta;
            this.glycanWeight = glycanWeight;
            this.coreGlycanWeight = coreGlycanWeight;
            this.peptideWeight = peptideWeight;

            glycanNum = (glycoPeptide as IGlycoPeptideMassProxy).GetMass(MassType.Glycan).Count;
            coreGlycanNum = (glycoPeptide as IGlycoPeptideMassProxy).GetMass(MassType.CoreGlycan).Count;
            peptideNum = (glycoPeptide as IGlycoPeptideMassProxy).GetMass(MassType.Peptide).Count;

            peakInclude = new HashSet<double>();
        }

        public void AddCoreScore(IPeak peak)
        {
            double mz = peak.GetMZ();
            if (!peakInclude.Contains(mz))
            {
                peakInclude.Add(mz);
                score += alpha * peak.GetIntensity() * Math.Pow(1.0 / coreGlycanNum, beta);
            }
        }

        public void AddPeptideScore(IPeak peak)
        {
            double mz = peak.GetMZ();
            if (!peakInclude.Contains(mz))
            {
                peakInclude.Add(mz);
                score += alpha * peak.GetIntensity() * Math.Pow(1.0 / peptideNum, beta);
            }
        }

        public void AddScore(IPeak peak)
        {
            double mz = peak.GetMZ();
            if (!peakInclude.Contains(mz))
            {
                peakInclude.Add(mz);
                score += alpha * peak.GetIntensity() * Math.Pow(1.0 / glycanNum, beta);
            }
        }

        public void AddScore(IScore other)
        {
           score += other.GetScore() ;
        }

        public IGlycoPeptide GetGlycoPeptide()
        {
            return glycoPeptide;
        }

        public double GetScore()
        {
            return score;
        }
    }
}
