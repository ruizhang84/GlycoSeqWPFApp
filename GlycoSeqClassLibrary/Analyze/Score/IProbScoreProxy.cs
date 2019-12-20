using GlycoSeqClassLibrary.Model.Spectrum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Analyze.Score
{
    public interface IProbScoreProxy: IFDRScoreProxy
    {
        double GetProbability();
        ISpectrum GetSpectrum();
    }
}
