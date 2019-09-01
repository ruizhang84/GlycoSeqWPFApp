using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Algorithm
{
    public interface IPoint : IComparable<IPoint>
    {
        double GetValue();
    }
}
