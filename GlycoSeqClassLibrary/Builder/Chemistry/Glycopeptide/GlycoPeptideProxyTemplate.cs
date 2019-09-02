using GlycoSeqClassLibrary.Model.Chemistry.Glycan;
using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using GlycoSeqClassLibrary.Model.Chemistry.Peptide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide
{
    public  class GlycoPeptideProxyTemplate : IGlycoPeptideProxy
    {
        protected IGlycoPeptide glycoPeptide;

        public GlycoPeptideProxyTemplate(IGlycoPeptide glycoPeptide)
        {
            this.glycoPeptide = glycoPeptide;
        }

        public virtual IGlycoPeptide Clone()
        {
            return glycoPeptide.Clone();
        }

        public IGlycan GetGlycan()
        {
            return glycoPeptide.GetGlycan();
        }

        public IPeptide GetPeptide()
        {
            return glycoPeptide.GetPeptide();
        }

        public int GetPosition()
        {
            return glycoPeptide.GetPosition();
        }

        public void SetGlycan(IGlycan glycan)
        {
            glycoPeptide.SetGlycan(glycan);
        }

        public void SetPeptide(IPeptide peptide)
        {
            glycoPeptide.SetPeptide(peptide);
        }

        public void SetPosition(int pos)
        {
            glycoPeptide.SetPosition(pos);
        }
    }
}
