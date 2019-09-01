using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Model.Chemistry.Peptide
{
    public class GeneralPeptide : IPeptide
    {
        protected string id;
        protected string sequence;

        public GeneralPeptide(string id, string seq)
        {
            this.id = id;
            sequence = seq;
        }


        public IPeptide Clone()
        {
            return new GeneralPeptide(id, sequence);
        }

        public string GetID()
        {
            return id;
        }

        public string GetSequence()
        {
            return sequence;
        }

        public void SetID(string id)
        {
            this.id = id;
        }

        public void SetSequence(string seq)
        {
            sequence = seq;
        }
    }
}
