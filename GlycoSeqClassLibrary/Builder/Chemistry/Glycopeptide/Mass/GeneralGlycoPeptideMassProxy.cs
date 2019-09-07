using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide.Mass
{
    public class GeneralGlycoPeptideMassProxy : GlycoPeptideProxyTemplate, IGlycoPeptideMassProxy
    {
        protected HashSet<double> massTable;

        public GeneralGlycoPeptideMassProxy(IGlycoPeptide glycoPeptide) : base(glycoPeptide)
        {
            massTable = new HashSet<double>();
        }

        public void AddMass(double mass)
        {
            massTable.Add(mass);
        }

        public void AddRangeMass(List<double> massList)
        {
            massTable.UnionWith(massList);
        }

        public void Clear()
        {
           massTable.Clear();
        }

        public List<double> GetMass()
        {
            return massTable.ToList().OrderBy(e => e).ToList();
        }
    }
}
