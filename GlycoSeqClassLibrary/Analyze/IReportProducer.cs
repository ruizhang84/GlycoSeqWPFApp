using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Analyze
{
    public interface IReportProducer
    {
        void Report(IResults results, int start, int end);
        void SetOutputLocation(string location);
    }
}
