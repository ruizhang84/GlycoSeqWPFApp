using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Analyze
{
    public interface IScoreFactory
    {
        IScore CreateScore(IGlycoPeptide glycoPeptide);
    }
}
