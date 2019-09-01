using GlycoSeqClassLibrary.Model.Chemistry.Glycan;
using GlycoSeqClassLibrary.Util.CalcMass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Glycan.Mass
{
    public class GeneralTableNGlycanMassProxy : TableNGlycanProxyTemplate, ITableNGlycanMassProxy
    {
        protected HashSet<double> massTable;

        public GeneralTableNGlycanMassProxy(ITableNGlycan glycan) : base(glycan)
        {
            massTable = new HashSet<double>();
            massTable.Add(GlycanCalcMass.Instance.Compute(glycan));
        }

        public List<double> GetSubTreeMass()
        {
            return massTable.ToList();
        }

        public void AddMass(double mass)
        {
            massTable.Add(mass);
        }

        public void AddRangeMass(List<double> massList)
        {
            massTable.UnionWith(massList);
        }

        public void ClearMass()
        {
            massTable.Clear();
        }

    }
}
