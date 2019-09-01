using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlycoSeqClassLibrary.Model.Chemistry.Glycan;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Glycan.Mass
{
    public class GeneralTableNGlycanMassProxyGenerator : ITableNGlycanProxyGenerator
    {
        protected int HexNAcBound;
        protected int HexBound;
        protected int FucBound;
        protected int NeuAcBound;
        protected int NeuGcBound;

        public GeneralTableNGlycanMassProxyGenerator(int hexNAc=12, int hex=12, int fuc=5, int neuAc=4, int neuGc=0)
        {
            HexNAcBound = hexNAc;
            HexBound = hex;
            FucBound = fuc;
            NeuAcBound = neuAc;
            NeuGcBound = neuGc;
        }

        public bool Criteria(ITableNGlycan glycan)
        {
            int[] composition = glycan.GetStructure();
            return composition[0] <= HexNAcBound && composition[1] <= HexBound
                && composition[2] <= FucBound && composition[3] <= NeuAcBound
                && composition[4] <= NeuGcBound;
        }

        public TableNGlycanProxyTemplate Generate(ITableNGlycan glycan)
        {
            return new GeneralTableNGlycanMassProxy(glycan);
        }

        public void Update(TableNGlycanProxyTemplate glycan, TableNGlycanProxyTemplate source)
        {
            (glycan as ITableNGlycanMassProxy).AddRangeMass((source as ITableNGlycanMassProxy).GetSubTreeMass());
        }
    }
}
