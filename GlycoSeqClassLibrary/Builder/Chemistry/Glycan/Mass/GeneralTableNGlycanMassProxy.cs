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
        protected HashSet<double> coreMassTable;

        public GeneralTableNGlycanMassProxy(ITableNGlycan glycan) : base(glycan)
        {
            massTable = new HashSet<double>();
            coreMassTable = new HashSet<double>();
            
            if (NotCore())
            {
                massTable.Add(GlycanCalcMass.Instance.Compute(glycan));
            }
            else
            {
                coreMassTable.Add(GlycanCalcMass.Instance.Compute(glycan));
            }     
        }

        private bool NotCore()
        {
            return glycan.GetNGlycanTable()[0] == 2 && glycan.GetNGlycanTable()[1] == 3;
        }

        public List<double> GetMass()
        {
            return massTable.ToList();
        }

        public List<double> GetCoreMass()
        {
            return coreMassTable.ToList();
        }

        public void AddMass(double mass)
        {
           massTable.Add(mass);
        }

        public void AddCoreMass(double mass)
        {
            coreMassTable.Add(mass);
        }

        public void AddRangeMass(List<double> massList)
        {
            massTable.UnionWith(massList);
        }

        public void AddCoreRangeMass(List<double> massList)
        {
            coreMassTable.UnionWith(massList);
        }

        public void ClearMass()
        {
            massTable.Clear();
        }

    }
}
