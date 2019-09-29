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

        public IScore Search(ISpectrum spectrum, IGlycoPeptide glycoPeptide)
        {
            List<IPoint> points = pointsCreator.Create(glycoPeptide);
            matcher.setData(points);
            IScore score = scoreFactory.CreateScore(glycoPeptide);

            double charge = (spectrum as ISpectrumMSn).GetParentCharge();
            foreach (IPeak peak in spectrum.GetPeaks())
            {
                for (int c = 1; c <= charge; c++)
                {
                    IPoint target = new GeneralPoint(peak.GetMZ() * c);
                    if (matcher.Found(target))
                    {
                        score.AddScore(peak);
                        break;
                    }
                }
            }
            return score;
        }
    }
}
  