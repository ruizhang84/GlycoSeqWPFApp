using GlycoSeqClassLibrary.Model.Chemistry.Peptide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Model.Chemistry.Protein
{
    public interface IProtein
    {
        string GetID();
        string GetSequence();
        List<IPeptide> GetPeptides();
        void SetID(string id);
        void SetSequence(string seq);
        void SetPeptides(List<IPeptide> peptides);
    }
}
