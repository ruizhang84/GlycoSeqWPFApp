using System;
using System.Collections.Generic;
using GlycoSeqClassLibrary.Model.Chemistry.Protein;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Protein
{
    public interface IProteinCreator
    {
        List<IProtein> Create(string fileName);
    }
}
