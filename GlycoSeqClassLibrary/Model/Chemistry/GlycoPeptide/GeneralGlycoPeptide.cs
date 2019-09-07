using GlycoSeqClassLibrary.Model.Chemistry.Glycan;
using GlycoSeqClassLibrary.Model.Chemistry.Peptide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide
{
    public class GeneralGlycoPeptide : IGlycoPeptide
    {
        protected IPeptide peptide;
        protected int position;
        protected IGlycan glycan;

        public GeneralGlycoPeptide(IGlycan glycan, IPeptide peptide, int pos)
        {
            this.peptide = peptide;
            this.glycan = glycan;
            this.position = pos;
        }

        public IGlycoPeptide Clone()
        {
            return new GeneralGlycoPeptide(glycan, peptide, position);
        }

        public IGlycan GetGlycan()
        {
            return glycan;
        }

        public IPeptide GetPeptide()
        {
            return peptide;
        }

        public int GetPosition()
        {
            return position;
        }

        public void SetGlycan(IGlycan glycan)
        {
            this.glycan = glycan;
        }

        public void SetPeptide(IPeptide peptide)
        {
            this.peptide = peptide;
        }

        public void SetPosition(int pos)
        {
            position = pos;
        }
    }
}
