using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlycoSeqClassLibrary.Algorithm;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide.Mass;
using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using GlycoSeqClassLibrary.Search.Precursor;

namespace GlycoSeqClassLibrary.Search.SearchEThcD
{
    public class GeneralGlycoPeptideMassProxyPointsCreator : IGlycoPeptidePointsCreator
    {
        public List<IPoint> Create(IGlycoPeptide glycoPeptide)
        {
            List<IPoint> points = new List<IPoint>();
            foreach(double mass in (glycoPeptide as IGlycoPeptideMassProxy).GetMass())
            {
                points.Add(new GeneralPoint(mass));
            }
            
            return points;
        }
    }
}
