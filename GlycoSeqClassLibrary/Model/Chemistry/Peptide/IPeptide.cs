using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Model.Chemistry.Peptide
{
    public interface IPeptide
    {
        string GetID();
        string GetSequence();
        void SetID(string id);
        void SetSequence(string seq);
        IPeptide Clone();

    }
}
