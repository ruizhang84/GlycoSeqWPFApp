using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Model.Chemistry.Glycan.TableNGlycan
{
    public partial class HybridNGlycan : ITableNGlycan
    {
        int branch;     //max 2
        int manBranch;  //max 2
        int[] table;    //GlcNAc(2) - Man(3) - Fuc(1) - GlcNAc(bisect,1) 0, 1, 2, 3
                        //[Man(branch1) - Man(branch2)]  4, 5
                        //[GlcNAc(branch1) - GlcNAc(branch2)] 6, 7
                        //[Gal(branch1) - Gal(branch2)]   8, 9
                        //[Fuc, Fuc]         10, 11
                        //[NeuAc, NeuAc]     12, 13
                        //[NeuGc, NeuGc]     14, 15
        protected string name;
        protected int[] composition;
        protected bool init;

        public HybridNGlycan(int[] structureTable)
        {
            table = structureTable.ToArray();
            branch = 2;
            manBranch = 2;
            composition = new int[5];
            init = false;
        }

        protected void InitGet()
        {
            composition[0] = table[0] + table[3] + table[6] + table[7];
            composition[1] = table[1] + table[4] + table[5] + table[8] + table[9] ;
            composition[2] = table[2] + table[10] + table[11];
            composition[3] = table[12] + table[13];
            composition[4] = table[14] + table[15];

            name = "Hybrid: ";
            if (table[2] > 0) name += "-fucose-";
            if (table[3] > 0) name += "-bisect-";
            name += "-core-" + string.Join(";", table.Take(2).ToArray())
                + "[" + string.Join(";", table.Skip(4).Take(2).ToArray()) + "]"
                + "[" + string.Join(";", table.Skip(6).Take(2).ToArray()) + "]"
                + "[" + string.Join(";", table.Skip(8).Take(2).ToArray()) + "]"
                + "[" + string.Join(";", table.Skip(10).Take(2).ToArray()) + "]"
                + "[" + string.Join(";", table.Skip(12).Take(2).ToArray()) + "]"
                + "[" + string.Join(";", table.Skip(14).Take(2).ToArray()) + "]";
            init = true;
        }

        public IGlycan Clone()
        {
            return new HybridNGlycan(table);
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
            return GlycanType.Hybrid;
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
            return new HybridNGlycan(table);
        }
    }
}
