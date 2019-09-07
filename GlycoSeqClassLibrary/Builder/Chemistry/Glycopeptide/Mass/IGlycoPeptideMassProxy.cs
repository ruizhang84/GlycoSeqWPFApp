using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide.Mass
{
    public interface IGlycoPeptideMassProxy : IGlycoPeptideProxy
    {
        List<double> GetMass();
        void AddMass(double mass);
        void AddRangeMass(List<double> massList);
        void Clear();

    }
}
