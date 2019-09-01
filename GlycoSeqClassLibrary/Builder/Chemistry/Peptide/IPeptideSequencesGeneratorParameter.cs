using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Peptide
{
    public enum Proteases { Trypsin, Pepsin, Chymotrypsin, GluC };
    public interface IPeptideSequencesGeneratorParameter
    {
        Proteases GetProtease();
        int GetMissCleavage();
        int GetMiniLength();
        void SetMiniLength(int length);
        void SetMissCleavage(int miss);
        void SetProtease(Proteases enzyme);
    }
}
