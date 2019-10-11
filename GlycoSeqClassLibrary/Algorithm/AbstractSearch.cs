using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Algorithm
{
    public abstract class AbstractSearch : ISearch
    {
        protected List<IPoint> data;
        protected IComparer<IPoint> comparer;

        public AbstractSearch(IComparer<IPoint> comparer)
        {
            this.comparer = comparer;
        }

        public virtual void setData(List<IPoint> data)
        {
            this.data = data;
            if (data.Count > 1)
                this.data.Sort();
        }

        public virtual int Compare(IPoint p1, IPoint p2)
        {
            return comparer.Compare(p1, p2);
        }

        public abstract bool Found(IPoint pt);
        public abstract List<IPoint> Search(IPoint pt);
    }
}
