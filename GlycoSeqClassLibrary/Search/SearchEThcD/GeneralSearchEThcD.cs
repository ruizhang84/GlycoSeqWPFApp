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

        protected void ComputeSearchScore(IScore score, List<IPeak> peaks,
            double charge, IGlycoPeptide glycoPeptide, MassType type)
        {
            List<IPoint> points = pointsCreator.Create(glycoPeptide, type);
            matcher.setData(points);

            foreach (IPeak peak in peaks)
            {
                for (int c = 1; c <= charge; c++)
                {
                    IPoint target = new GeneralPoint(SpectrumCalcMass.Instance.Compute(peak.GetMZ(), c));
                    if (matcher.Found(target))
                    {
                        Console.WriteLine(glycoPeptide.GetPeptide().GetSequence());
                        Console.WriteLine(glycoPeptide.GetGlycan().GetName());
                        Console.WriteLine(peak.GetMZ().ToString() + " " + c.ToString() + ": " + type.ToString());
                        switch (type)
                        {
                            case MassType.Core:
                                score.AddCoreScore(peak);
                                break;
                            case MassType.Branch:
                                score.AddBranchScore(peak);
                                break;
                            case MassType.Glycan:
                                score.AddScore(peak);
                                break;
                            case MassType.Peptide:
                                score.AddPeptideScore(peak);
                                break;
                            default:
                                break;
                        }
                        break;
                    }
                }
            }
        }

        public IScore Search(ISpectrum spectrum, IGlycoPeptide glycoPeptide)
        {
            double charge = (spectrum as ISpectrumMSn).GetParentCharge();
            List<IPeak> peaks = spectrum.GetPeaks();
            IScore score = scoreFactory.CreateScore(glycoPeptide);

            ComputeSearchScore(score, peaks, charge, glycoPeptide, MassType.Glycan);
            ComputeSearchScore(score, peaks, charge, glycoPeptide, MassType.Core);
            ComputeSearchScore(score, peaks, charge, glycoPeptide, MassType.Branch);
            ComputeSearchScore(score, peaks, charge, glycoPeptide, MassType.Peptide);
            return score;
        }
    }
}
  