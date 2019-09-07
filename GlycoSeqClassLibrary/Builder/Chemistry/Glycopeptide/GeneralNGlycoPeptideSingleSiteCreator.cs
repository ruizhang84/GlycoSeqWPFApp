using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlycoSeqClassLibrary.Model.Chemistry.Glycan;
using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using GlycoSeqClassLibrary.Model.Chemistry.Peptide;
using GlycoSeqClassLibrary.Util.Model;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Glycopeptide
{
    public class GeneralNGlycoPeptideSingleSiteCreator : IGlycoPeptideCreator
    {
        protected IGlycoPeptideProxyGenerator generator;
        public GeneralNGlycoPeptideSingleSiteCreator(IGlycoPeptideProxyGenerator generator)
        {
            this.generator = generator;
        }

        public List<IGlycoPeptide> Create(IGlycan glycan, IPeptide peptide)
        {
            List<IGlycoPeptide> glycoPeptides = new List<IGlycoPeptide>();
            foreach(int pos in FindPTMPosition.FindNGlycanSite(peptide.GetSequence()))
            {
                try
                {
                    IGlycoPeptideProxy glycoPeptideProxy = generator.Generate(glycan, peptide, pos);
                    glycoPeptides.Add(glycoPeptideProxy);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return glycoPeptides;
        }
    }
}
