using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlycoSeqClassLibrary.Algorithm;
using GlycoSeqClassLibrary.Analyze;
using GlycoSeqClassLibrary.Analyze.Score;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide.Mass;
using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using GlycoSeqClassLibrary.Model.Spectrum;
using GlycoSeqClassLibrary.Search.Precursor;
using GlycoSeqClassLibrary.Util.CalcMass;

namespace GlycoSeqClassLibrary.Search.SearchEThcD
{
    public class GeneralSearchEThcD : ISearchEThcD
    {
        ISearch matcher;
        IScoreFactory scoreFactory;
        IGlycoPeptidePointsCreator pointsCreator;

        public GeneralSearchEThcD(ISearch matcher, IScoreFactory scoreFactory, IGlycoPeptidePointsCreator glycoPeptidePointsCreator)
        {
            this.matcher = matcher;
            this.scoreFactory = scoreFactory;
            pointsCreator = glycoPeptidePointsCreator;
        }

        protected IScore ComputeSearchScore(List<IPeak> peaks, double charge, IGlycoPeptide glycoPeptide, MassType type)
        {
            List<IPoint> points = pointsCreator.Create(glycoPeptide, type);
            matcher.setData(points);
            IScore score = scoreFactory.CreateScore(glycoPeptide);

            foreach (IPeak peak in peaks)
            {
                for (int c = 1; c <= charge; c++)
                {
                    IPoint target = new GeneralPoint(SpectrumCalcMass.Instance.Compute(peak.GetMZ(), c));
                    if (matcher.Found(target))
                    {
                        switch (type)
                        {
                            case MassType.CoreGlycan:
                                score.AddCoreScore(peak);
                                break;
                            case MassType.Glycan:
                                score.AddScore(peak);
                                break;
                            case MassType.Peptide:
                                score.AddPeptideScore(peak);
                                break;
                        }
                        break;
                    }
                }
            }
            return score;
        }

        public IScore Search(ISpectrum spectrum, IGlycoPeptide glycoPeptide)
        {
            double charge = (spectrum as ISpectrumMSn).GetParentCharge();
            List<IPeak> peaks = spectrum.GetPeaks();
            IScore score = ComputeSearchScore(peaks, charge, glycoPeptide, MassType.Glycan);
            IScore peptideScore = ComputeSearchScore(peaks, charge, glycoPeptide, MassType.Peptide);
            score.AddScore(peptideScore);

            return score;
        }

        public IScore SearchDecoy(ISpectrum spectrum, IGlycoPeptide glycoPeptide)
        {
            double charge = (spectrum as ISpectrumMSn).GetParentCharge();
            List<IPeak> peaks = spectrum.GetPeaks();
            IScore score = ComputeSearchScore(peaks, charge, glycoPeptide, MassType.Glycan);
            IScore peptideScore = ComputeSearchScore(peaks, charge, glycoPeptide, MassType.Peptide);
            score.AddScore(peptideScore);

            return score;
        }
    }
}
  