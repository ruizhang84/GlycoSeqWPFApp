using GlycoSeqClassLibrary.Builder.Chemistry.Glycan;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycan.Mass;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide;
using GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide.Mass;
using GlycoSeqClassLibrary.Model.Chemistry.Glycan;
using GlycoSeqClassLibrary.Model.Chemistry.Glycan.TableNGlycan;
using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using GlycoSeqClassLibrary.Model.Chemistry.Peptide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    class TestCase3 : TestCase
    {
        public void Run()
        {
            ITableNGlycan glycan = new ComplexNGlycan(new int[24]);
            Console.WriteLine(glycan.GetName());
            //glycan.SetNGlycanTable(0, 1);
            ITableNGlycanProxy glycanProxy = new GeneralTableNGlycanMassProxy(glycan);

            ITableNGlycanProxyGenerator generator = new GeneralTableNGlycanMassProxyGenerator();
            IGlycanCreator glycanCreator = new GeneralTableNGlycanCreator(generator, glycanProxy);
            List<IGlycan> glycans = glycanCreator.Create();
            //foreach(IGlycan g in glycans)
            //{
            //    Console.WriteLine(string.Join(" ", g.GetStructure()));
            //}

            IGlycoPeptideProxyGenerator glycoPeptideProxyGenerator = new GeneralTableNGlycoPeptideMassProxyGenerator();
            IGlycoPeptideCreator glycoPeptideCreator = new GeneralNGlycoPeptideSingleSiteCreator(glycoPeptideProxyGenerator);

            IPeptide peptide = new GeneralPeptide("test3", "ILGGHLDAKGSFPWQAKMVSHHNLTTGATLINEQWLLTTAK");
            foreach(IGlycan g in glycans)
            {
                List<IGlycoPeptide> glycoPeptides = glycoPeptideCreator.Create(g, peptide);
                //Console.WriteLine(glycoPeptides.Count);

                foreach(IGlycoPeptide glyco in glycoPeptides)
                {
                    //Console.WriteLine(glyco.GetGlycan().GetName());
                    //Console.WriteLine(glyco.GetPosition());
                }
            }

            Console.Read();
        }
    }
}
