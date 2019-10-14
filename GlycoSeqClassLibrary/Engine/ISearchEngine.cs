using GlycoSeqClassLibrary.Analyze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Engine
{
    public delegate void progress(int scan);

    public interface ISearchEngine
    {
        void Init(string spectrumFileLocation, string peptideFileLocation, string outputLocation);
        void Analyze(int start, int end);
        void Analyze(int start, int end, IResults results);
        void Search(int start, int end, progress sender);
        int GetFirstScan();
        int GetLastScan();
        IResults GetResults();
    }
}
