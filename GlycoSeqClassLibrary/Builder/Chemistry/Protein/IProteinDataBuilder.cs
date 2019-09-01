using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Protein
{
    public interface IProteinEntry
    {
        string GetID();
        string GetSequence();
        void SetID(string id);
        void SetSequence(string seq);
    }

    public interface IProteinDataBuilder
    {
        void Read(string fileName);

        List<IProteinEntry> GetEntries();
    }
}
