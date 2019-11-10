using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using GlycoSeqClassLibrary.Model.Spectrum;

namespace GlycoSeqClassLibrary.Analyze
{
    public interface IScore
    {
        IGlycoPeptide GetGlycoPeptide();
        double GetScore();
        void AddScore(IPeak peak);
        void AddCoreScore(IPeak peak);
        void AddBranchScore(IPeak peak);
        void AddPeptideScore(IPeak peak);
        void AddScore(IScore other);
    }
}
