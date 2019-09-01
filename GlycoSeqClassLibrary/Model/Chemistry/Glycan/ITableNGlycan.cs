using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Model.Chemistry.Glycan
{
    public enum MonosaccharideType
    {
        GlcNAc, Man, Gal, Fuc, NeuAc, NeuGc
        //GlcNAc, Man, Gal, Fuc, NeuAc, NeuGc, GalNAc, GalN, Glc, GlcN, ManNAc, ManN,
        //Xyl, Kdn, GlcA, IdoA, GalA, ManA
    }

    public interface ITableNGlycan : IGlycan
    {
        GlycanType GetNGlycanType();
        ITableNGlycan TableClone();
        List<ITableNGlycan> Growth(MonosaccharideType suger);
        int[] GetNGlycanTable();
        void SetNGlycanTable(int indx, int num);
    }
}
