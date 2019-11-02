using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide.Mass
{
    public enum MassType { Glycan, CoreGlycan, Peptide, All}
    public interface IGlycoPeptideMassProxy : IGlycoPeptideProxy
    {
        List<double> GetMass(MassType type);
        void AddMass(double mass, MassType type);
        void AddRangeMass(List<double> massList, MassType type);
        void Clear();

    }
}
