﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Algorithm
{
    public class BinarySearch : AbstractSearch, ISearch
    {
        public BinarySearch(List<IPoint> data, double tol) : base(data, tol) { }

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
                else if (cmp < 0)
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

        public List<IPoint> ExtendAllMatch(IPoint pt, int matchIndx)
        {
            List<IPoint> searched = new List<IPoint>();
            if (matchIndx < 0) return searched;
           
            for(int left = matchIndx; left >= 0 && Compare(data[left], pt) == 0; left--)
            {
                searched.Add(data[left]);
            }

            for (int right = matchIndx+1; right < data.Count && Compare(data[right], pt) == 0; right++)
            {
                searched.Add(data[right]);
            }
     
            return searched;
        }
    }
}