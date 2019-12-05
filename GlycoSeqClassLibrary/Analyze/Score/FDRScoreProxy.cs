using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide.Mass;
using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using GlycoSeqClassLibrary.Model.Spectrum;

namespace GlycoSeqClassLibrary.Analyze.Score
{
    public class FDRScoreProxy : IFDRScoreProxy
    {
        IScore score;
        bool ndecoy;

        public FDRScoreProxy(IScore score, bool ndecoy)
        {
            this.score = score;
            this.ndecoy = ndecoy;
        }

        public bool IsDecoy()
        {
            return !ndecoy;
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
    }
}
