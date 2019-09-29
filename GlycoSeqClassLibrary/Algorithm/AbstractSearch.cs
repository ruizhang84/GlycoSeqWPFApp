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
        public double Tolerance { get; set; }

        public AbstractSearch(List<IPoint> data, double tol)
        {
            this.data = data;
            if (data.Count > 1)
                this.data.Sort();
            Tolerance = tol;
        }

        public virtual void setData(List<IPoint> data)
        {
            this.data = data;
            if (data.Count > 1)
                this.data.Sort();
        }

        public virtual int Compare(IPoint p1, IPoint p2)
        {
            if (Math.Abs(p1.GetValue()-p2.GetValue()) < Tolerance) return 0;
            else if (p1.GetValue() < p2.GetValue()) return -1;
            return 1;
        }

        public abstract bool Found(IPoint pt);
        public abstract List<IPoint> Search(IPoint pt);
    }
}
