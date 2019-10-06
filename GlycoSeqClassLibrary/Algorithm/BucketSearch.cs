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
        protected double Tolerance { get; set;}
        protected bool searchable;
        
        public BucketSearch(List<IPoint> data, IComparer<IPoint> comparer, double tol = 0.1) : base(data, comparer)
        {
            Tolerance = tol;
            if (data.Count > 0)
            {
                double lowerBound = data.Min(x => x.GetValue());
                double upperBound = data.Max(x => x.GetValue());
                Init(lowerBound, upperBound);
                AddRange(data);
                searchable = true;
            }
            else
            {
                searchable = false;
            }
        }

        public override void setData(List<IPoint> data)
        {
            if (data.Count > 0)
            {
                double lowerBound = data.Min(x => x.GetValue());
                double upperBound = data.Max(x => x.GetValue());
                Init(lowerBound, upperBound);
                AddRange(data);
                searchable = true;
            }
            else
            {
                searchable = false;
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
            return (int) Math.Ceiling((pt.GetValue() - minValue) / bucketSize);
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

        public override bool Found(IPoint pt)
        {
            if (!searchable) return false;

            int index = FindBucket(pt);
            if (index >= 0 && index < pointTable.Length && pointTable[index].Count > 0)
                return true;

            for (int i = -1; i <= 1; i++)
            {
                if (i == 0 || index + i < 0 || index + i > pointTable.Length)
                    continue;
                
                foreach (IPoint p in pointTable[index + i])
                {
                    if (Compare(pt, p) == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private List<IPoint> Match(IPoint pt)
        {
            List<IPoint> result = new List<IPoint>();
            if (!searchable) return result;
            int index = FindBucket(pt);

            for (int i = -1; i <= 1; i++)
            {
                if (index + i < 0 || index + i > pointTable.Length)
                    continue;
                foreach (IPoint p in pointTable[index + i])
                {
                    if (Compare(pt, p) == 0)
                    {
                        result.Add(p);
                    }
                }
            }
            return result;
        }
    
    }   
}
