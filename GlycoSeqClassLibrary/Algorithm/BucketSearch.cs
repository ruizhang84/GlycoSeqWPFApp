using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Algorithm
{
    public class BucketSearch : AbstractSearch, ISearch
    {
        protected List<IPoint>[] pointTable;
        protected double bucketSize;
        protected double minValue;
        
        public BucketSearch(List<IPoint> data, double tol) : base(data, tol)
        {
            if (data.Count > 0)
            {
                double lowerBound = data[0].GetValue();
                double upperBound = data[data.Count-1].GetValue();
                Init(lowerBound, upperBound);
                AddRange(data);
            }
        }

        public override void setData(List<IPoint> data)
        {
            base.setData(data);
            if (data.Count > 0)
            {
                double lowerBound = data[0].GetValue();
                double upperBound = data[data.Count - 1].GetValue();
                Init(lowerBound, upperBound);
                AddRange(data);
            }

        }

        public override List<IPoint> Search(IPoint pt)
        {

            return Match(pt);
        }

        private void Init(double lowerBound, double upperBound)
        {
            //sanity check
            bucketSize = Tolerance == 0 ? 1.0 : Tolerance;

            int bucketNums = (int) Math.Ceiling((upperBound - lowerBound + 1) / bucketSize);
            pointTable = new List<IPoint>[bucketNums];
            for(int i = 0; i < bucketNums; i++)
            {
                pointTable[i] = new List<IPoint>();
            }
            minValue = lowerBound;
        }

        private int FindBucket(IPoint pt)
        {
            return (int)Math.Ceiling((pt.GetValue() - minValue) / bucketSize);
        }


        private void Add(IPoint pt)
        {
            pointTable[FindBucket(pt)].Add(pt);
        }

        private void AddRange(List<IPoint> points)
        {
            foreach (IPoint pt in points)
            {
                Add(pt);
            }
        }

        private List<IPoint> Match(IPoint pt)
        {
            List<IPoint> result = new List<IPoint>();
            int index = FindBucket(pt);

            for (int i = -1; i <= 1; i++)
            {
                if (index + i < 0 || index + i > pointTable.Length)
                    continue;
                foreach (IPoint p in pointTable[index + i])
                {
                    if (Compare(p, pt) == 0)
                    {
                        result.Add(p);
                    }
                }
            }
            return result;
        }
    
}
}
