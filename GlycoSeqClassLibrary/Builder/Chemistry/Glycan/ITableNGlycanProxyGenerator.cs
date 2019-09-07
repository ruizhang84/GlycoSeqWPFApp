using GlycoSeqClassLibrary.Model.Chemistry.Glycan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Glycan
{
    public interface ITableNGlycanProxyGenerator
    {
        bool Criteria(ITableNGlycan glycan);
        ITableNGlycanProxy Generate(ITableNGlycan glycan);
        void Update(ITableNGlycanProxy glycan, ITableNGlycanProxy source);
    }
}
