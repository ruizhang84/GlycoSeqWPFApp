using GlycoSeqClassLibrary.Model.Chemistry.Protein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Protein.Fasta
{
    public class GeneralProteinCreator : IProteinCreator
    {
        IProteinDataBuilder builder;

        public GeneralProteinCreator(IProteinDataBuilder builder)
        {
            this.builder = builder;
        }

        public List<IProtein> Create(string fileName)
        {
            List<IProtein> proteins = new List<IProtein>();
            builder.Read(fileName);

            foreach (IProteinEntry entry in builder.GetEntries())
            {
                proteins.Add(new GeneralProtein(entry.GetID(), entry.GetSequence()));
            }

            return proteins;
        }
    }
}
