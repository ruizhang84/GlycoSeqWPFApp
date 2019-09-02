using GlycoSeqClassLibrary.Model.Chemistry.Glycan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Glycan.Mass
{
    public interface ITableNGlycanMassProxy : ITableNGlycanProxy
    {
        List<double> GetMass();
        void AddMass(double mass);
        void AddRangeMass(List<double> massList);
        void ClearMass();
    }
}
