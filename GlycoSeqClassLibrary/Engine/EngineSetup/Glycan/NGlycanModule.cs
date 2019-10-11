using Autofac;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycan;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycan.Mass;
using GlycoSeqClassLibrary.Model.Chemistry.Glycan.TableNGlycan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Engine.EngineSetup.Glycan
{
    public class NGlycanModule : Module
    {
        public int HexNAcBound { get; set; }
        public int HexBound { get; set; }
        public int FucBound { get; set; }
        public int NeuAcBound { get; set; }
        public int NeuGcBound { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                ITableNGlycanProxyGenerator tableNGlycanProxyGenerator =
                    new GeneralTableNGlycanMassProxyGenerator(HexNAcBound, HexBound, FucBound, NeuAcBound, NeuGcBound);
                int[] structTable = new int[24];
                structTable[0] = 1;
                ITableNGlycanProxy root = new GeneralTableNGlycanMassProxy(new ComplexNGlycan(structTable));
                return new GeneralTableNGlycanCreator(tableNGlycanProxyGenerator, root);
            }).As<IGlycanCreator>();
        }
    }
}
