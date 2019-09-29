using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Algorithm
{
    public class BinarySearch : AbstractSearch, ISearch
    {
        public BinarySearch(List<IPoint> data, IComparer<IPoint> comparer) : base(data, comparer)
        {
            if (data.Count > 1)
                data.Sort();
        }

        public override List<IPoint> Search(IPoint pt)
        {
            return ExtendAllMatch(pt, BinarySearchPoints(pt));
        }

        public int BinarySearchPoints(IPoint pt)
        {
            int start = 0;
            int end = data.Count - 1;
            
            while (start <= end)
            {
                int mid = end + (start - end) / 2;
                int cmp = Compare(data[mid], pt);
                if (cmp == 0)
                {
                    return mid;
                }
                else if (cmp > 0)
                {
                    end = mid - 1;
                }
                else
                {
                    start = mid + 1;
                }
            }

            return -1;
        }

        public override bool Found(IPoint pt)
        {
            return BinarySearchPoints(pt) >= 0;
        }

        public List<IPoint> ExtendAllMatch(IPoint pt, int matchIndx)
        {
            List<IPoint> searched = new List<IPoint>();
            if (matchIndx < 0) return searched;
           
            for(int left = matchIndx; left >= 0 && Compare(pt, data[left]) == 0; left--)
            {
                searched.Add(data[left]);
            }

            for (int right = matchIndx+1; right < data.Count && Compare(pt, data[right]) == 0; right++)
            {
                searched.Add(data[right]);
            }
     
            return searched;
        }
    }
}
