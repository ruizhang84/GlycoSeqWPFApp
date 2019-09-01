using GlycoSeqClassLibrary.Model.Chemistry.Peptide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Model.Chemistry.Protein
{
    public class GeneralProtein : IProtein
    {
        protected List<IPeptide> peptides;
        protected string id;
        protected string sequence;

        public GeneralProtein(string id, string seq)
        {
            this.id = id;
            sequence = seq;
            peptides = new List<IPeptide>();
        }

        public string GetID()
        {
            return id;
        }

        public List<IPeptide> GetPeptides()
        {
            return peptides;
        }

        public string GetSequence()
        {
            return sequence;
        }

        public void SetID(string id)
        {
            this.id = id;
        }

        public void SetPeptides(List<IPeptide> peptides)
        {
            this.peptides = peptides;
        }

        public void SetSequence(string seq)
        {
            sequence = seq;
        }
    }
}
