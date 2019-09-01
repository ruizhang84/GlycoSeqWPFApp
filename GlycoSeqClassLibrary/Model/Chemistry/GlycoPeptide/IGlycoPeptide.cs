using GlycoSeqClassLibrary.Model.Chemistry.Glycan;
using GlycoSeqClassLibrary.Model.Chemistry.Peptide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide
{
    public interface IGlycoPeptide
    {
        IPeptide GetPeptide();
        IGlycan GetGlycan();
        int GetPosition();
        IGlycoPeptide Clone();
        void SetGlycan(IGlycan glycan);
        void SetPeptide(IPeptide peptide);
        void SetPosition(int pos);
    }
}
