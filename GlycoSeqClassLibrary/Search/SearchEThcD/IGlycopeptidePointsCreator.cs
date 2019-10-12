using GlycoSeqClassLibrary.Algorithm;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide.Mass;
using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Search.SearchEThcD
{
    public interface IGlycoPeptidePointsCreator
    {
        List<IPoint> Create(IGlycoPeptide glycopeptide, MassType massType);
    }
}
