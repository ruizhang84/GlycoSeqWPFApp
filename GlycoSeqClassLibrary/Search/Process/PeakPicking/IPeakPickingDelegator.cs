using GlycoSeqClassLibrary.Model.Spectrum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Search.Process.PeakPicking
{
    public interface IPeakPickingDelegator
    {
        List<IPeak> Picking(List<IPeak> peaks);
    }
}
