using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlycoSeqClassLibrary.Model.Spectrum;

namespace GlycoSeqClassLibrary.Analyze.Result
{
    public class GeneralResults : IResults
    {
        Dictionary<int, ISpectrum> spectrumTable;
        Dictionary<int, List<IScore>> scoreTable;

        public GeneralResults()
        {
            spectrumTable = new Dictionary<int, ISpectrum>();
            scoreTable = new Dictionary<int, List<IScore>>();
        }

        public void Clear()
        {
            spectrumTable.Clear();
            scoreTable.Clear();
        }

        public List<IScore> GetResult(int scanNum)
        {
            return scoreTable[scanNum];
        }

        public ISpectrum GetSpectrum(int scanNum)
        {
            return spectrumTable[scanNum];
        }

        public bool Contains(int scanNum)
        {
            return spectrumTable.ContainsKey(scanNum);
        }

        public void Add(ISpectrum spectrum, List<IScore> score)
        {
            int scan = spectrum.GetScanNum();
            spectrumTable[scan] = spectrum;
            scoreTable[scan] = score;
        }
    }
}
