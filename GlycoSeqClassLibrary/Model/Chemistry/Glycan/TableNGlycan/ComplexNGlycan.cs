using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Model.Chemistry.Glycan.TableNGlycan
{
    public partial class ComplexNGlycan : AbstractNGlycan, ITableNGlycan
    {
        public ComplexNGlycan(int[] structureTable) : base(structureTable)
        {
            //GlcNAc(2) - Man(3) - Fuc(1) - GlcNAc(bisect,1) -0,1,2,3
            //[GlcNAc(branch1) - GlcNAc(branch2) - GlcNAc(branch3) - GlcNAc(branch4)] -4,5,6,7
            //[Gal(branch1) - Gal(branch2) - Gal(branch3) - Gal(branch4)] -8,9,10,11
            //[Fuc(branch1) - Fuc(branch2) - Fuc(branch3) - Fuc(branch4)] -12,13,14,15
            //[NeuAc(branch1) - NeuAc(branch2) - NeuAc(branch3) - NeuAc(branch4)] -16,17,18,19
            //[NeuGc(branch1) - NeuGc(branch2) - NeuGc(branch3) - NeuGc(branch4)] -20,21,22,23
        }

        public override int[] GetStructure()
        {   
            int[] composition = new int[5];
            composition[0] = table[0] + table[3] + table[4] + table[5] + table[6] + table[7];
            composition[1] = table[1] + table[8] + table[9] + table[10] + table[11];
            composition[2] = table[2] + table[12] + table[13] + table[14] + table[15];
            composition[3] = table[16] + table[17] + table[18] + table[19];
            composition[4] = table[20] + table[21] + table[22] + table[23];
            return composition;
        }

        public override GlycanType GetNGlycanType()
        {
            return GlycanType.Complex;
        }

        public override IGlycan Clone()
        {
            return new ComplexNGlycan(table);
        }

        public override ITableNGlycan TableClone()
        {
            return new ComplexNGlycan(table);
        }

        //public string GetPrintString()
        //{
        //    name = "ComplexNGlycan: ";
        //    int[] composition = GetStructure();
        //    name = "[" + string.Join(",", composition) + "]";
        //    if (table[2] > 0) name += "-fucose-";
        //    if (table[3] > 0) name += "-bisect-";
        //    name += "-core-" + string.Join(";", table.Take(2).ToArray())
        //        + "[" + string.Join(";", table.Skip(4).Take(4).ToArray()) + "]"
        //        + "[" + string.Join(";", table.Skip(8).Take(4).ToArray()) + "]"
        //        + "[" + string.Join(";", table.Skip(12).Take(4).ToArray()) + "]"
        //        + "[" + string.Join(";", table.Skip(16).Take(4).ToArray()) + "]"
        //        + "[" + string.Join(";", table.Skip(20).Take(4).ToArray()) + "]";
        //    return name;
        //}
    }
}
