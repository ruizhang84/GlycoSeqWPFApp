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
        protected Dictionary<MassType, HashSet<double>> massTable;

        public GeneralGlycoPeptideMassProxy(IGlycoPeptide glycoPeptide) : base(glycoPeptide)
        {
            massTable = new Dictionary<MassType, HashSet<double>>();
        }

        public void AddMass(double mass, MassType type)
        {
            if (!massTable.ContainsKey(type))
            {
                massTable[type] = new HashSet<double>();
            }
            massTable[type].Add(mass);
        }

        public void AddRangeMass(List<double> massList, MassType type)
        {
            if (!massTable.ContainsKey(type))
            {
                massTable[type] = new HashSet<double>();
            }

            massTable[type].UnionWith(massList);
        }

        public void Clear()
        {
           massTable.Clear();
        }

        public List<double> GetMass(MassType type)
        {
            if (massTable.ContainsKey(type))
            {
                return massTable[type].ToList();
            }
            return new List<double>();
        }
    }
}
