using GlycoSeqClassLibrary.Model.Chemistry.Glycan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Glycan
{
    public class TableNGlycanProxyTemplate : ITableNGlycanProxy
    {
        protected ITableNGlycan glycan;

        public TableNGlycanProxyTemplate(ITableNGlycan glycan)
        {
            this.glycan = glycan;
        }

        public IGlycan Clone()
        {
            return glycan.Clone();
        }

        public string GetName()
        {
            return glycan.GetName();
        }

        public int[] GetNGlycanTable()
        {
            return glycan.GetNGlycanTable();
        }

        public GlycanType GetNGlycanType()
        {
            return glycan.GetNGlycanType();
        }

        public int[] GetStructure()
        {
            return glycan.GetStructure();
        }

        public List<ITableNGlycan> Growth(MonosaccharideType suger)
        {
            return glycan.Growth(suger);
        }

        public void SetNGlycanTable(int indx, int num)
        {
            glycan.SetNGlycanTable(indx, num);
        }

        public ITableNGlycan TableClone()
        {
            return glycan.TableClone();
        }

    }
}
