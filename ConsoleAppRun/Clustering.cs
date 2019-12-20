using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleAppRun.Program;

namespace ConsoleAppRun
{
    public class Clustering
    {
        public static int GetParent(int num, Dictionary<int, int> parent)
        {
            if (parent[num] == num) return num;
            return GetParent(parent[num], parent);
        }
        public static void GetCluster(List<CosInfo> cosInfos,
            Dictionary<int, List<int>> clusters,
            Dictionary<int, string> names,
            double cutoff)
        {
            Dictionary<int, int> parent = new Dictionary<int, int>();
            Dictionary<int, int> rank = new Dictionary<int, int>();


            foreach (CosInfo info in cosInfos)
            {
                if (info.cos >= cutoff)
                {
                    if (!parent.ContainsKey(info.ScanA))
                    {
                        parent[info.ScanA] = info.ScanA;
                        names[info.ScanA] = info.Name;
                        rank[info.ScanA] = 1;
                    }
                    if (!parent.ContainsKey(info.ScanB))
                    {
                        parent[info.ScanB] = info.ScanB;
                        names[info.ScanB] = info.Name;
                        rank[info.ScanB] = 1;
                    }

                    // not in the same group, merge
                    int parentA = GetParent(info.ScanA, parent);
                    int parentB = GetParent(info.ScanB, parent);
                    if (parentA != parentB)
                    {
                        if (rank[parentA] > rank[parentB])
                        {
                            parent[parentA] = parentB;
                        }
                        else
                        {
                            if (rank[parentA] == rank[parentB])
                            {
                                rank[parentA]++;
                            }
                            parent[parentB] = parentA;
                        }
                    }
                }
            }

            foreach (int scan in parent.Keys)
            {
                int p = GetParent(scan, parent);
                if (!clusters.ContainsKey(p))
                {
                    clusters[p] = new List<int>();
                }
                clusters[p].Add(scan);
            }
        }
    }
}
