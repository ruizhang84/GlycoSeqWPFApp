using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Model.Chemistry.Glycan.TableNGlycan
{
    public partial class Oligomannose : ITableNGlycan
    {
        int branch;              // max 3
        int[] table;             // GlcNAc(2) - Man(3) - Fuc - [Man(branch1) - Man(branch2) - Man(branch3)] 0 1 2 3 4 5
        protected string name;
        protected int[] composition;
        protected bool init;

        public Oligomannose(int[] structureTable)
        {
            table = structureTable.ToArray();
            branch = 3;
            composition = new int[5];
            init = false;
        }

        protected void InitGet()
        {
            composition[0] = table[0];
            composition[1] = table[1] + table[3] + table[4] + table[5];
            composition[2] = table[2];
            composition[3] = 0;
            composition[4] = 0;

            name = "OligoMannose: ";
            if (table[2] > 0) name += "-fucose-";
            name += "-core-" + string.Join(";", table.Take(2).ToArray())
                + "[" + string.Join(";", table.Skip(3).Take(3).ToArray()) + "]";
            init = true;
        }

        public IGlycan Clone()
        {
            return new Oligomannose(table);
        }

        public string GetName()
        {
            if (!init)
                InitGet();
            return name;
        }

        public int[] GetNGlycanTable()
        {
            return table;
        }

        public GlycanType GetNGlycanType()
        {
            return GlycanType.Oligomannose;
        }

        public int[] GetStructure()
        {
            if (!init)
                InitGet();
            return composition;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetNGlycanTable(int idx, int num)
        {
            table[idx] = num;
        }

        public ITableNGlycan TableClone()
        {
            return new Oligomannose(table);
        }
    }
}
