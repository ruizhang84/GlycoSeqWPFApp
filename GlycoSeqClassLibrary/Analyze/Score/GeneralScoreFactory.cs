using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;

namespace GlycoSeqClassLibrary.Analyze.Score
{
    public class GeneralScoreFactory : IScoreFactory
    {
        double alpha;
        double beta;

        public GeneralScoreFactory(double alpha, double beta)
        {
            this.alpha = alpha;
            this.beta = beta;
        }

        public IScore CreateScore(IGlycoPeptide glycoPeptide, object parameter)
        {
            return new GeneralScore(glycoPeptide, alpha, beta);
        }
    }
}
