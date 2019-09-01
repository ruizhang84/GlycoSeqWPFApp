using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Model.Chemistry.Glycan.TableNGlycan
{
    public abstract class AbstractNGlycan : ITableNGlycan
    {
        protected int[] table;    
        protected string name;

        public AbstractNGlycan(int[] structureTable)
        {
            table = structureTable.ToArray();
            name = string.Join(" ", table);
        }
        public abstract int[] GetStructure();

        public abstract GlycanType GetNGlycanType();

        public abstract IGlycan Clone();

        public abstract ITableNGlycan TableClone();

        public abstract List<ITableNGlycan> Growth(MonosaccharideType suger);

        public int[] GetTable()
        {
            return table;
        }

        public string GetName()
        {
            return name;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public int[] GetNGlycanTable()
        {
            return table;
        }

        public void SetNGlycanTable(int idx, int num)
        {
            table[idx] = num;
        }


    }
}
