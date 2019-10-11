using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Engine
{
    public interface ISearchEngine
    {
        void Init(string spectrumFileLocation, string peptideFileLocation, string outputLocation);
        void Analyze(int start, int end);
        void Search(int scan);
        int GetFirstScan();
        int GetLastScan();
    }
}
