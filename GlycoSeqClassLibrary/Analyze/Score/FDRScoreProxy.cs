using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public IGlycoPeptide GetGlycoPeptide()
        {
            return score.GetGlycoPeptide();
        }

        public double GetScore()
        {
            return score.GetScore();
        }
    }
}
