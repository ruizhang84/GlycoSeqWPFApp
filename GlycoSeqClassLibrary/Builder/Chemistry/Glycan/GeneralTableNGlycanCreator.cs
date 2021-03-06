﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlycoSeqClassLibrary.Model.Chemistry.Glycan;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Glycan
{
    public class GeneralTableNGlycanCreator : IGlycanCreator
    {
        protected ITableNGlycanProxyGenerator generator;
        protected ITableNGlycanProxy root;

        public GeneralTableNGlycanCreator(ITableNGlycanProxyGenerator generator, ITableNGlycanProxy glycan)
        {
            this.generator = generator;
            root = glycan;
        }

        public List<IGlycan> Create()
        {
            
            Queue<ITableNGlycanProxy> queue = new Queue<ITableNGlycanProxy>();
            Dictionary<string, ITableNGlycanProxy> visited = new Dictionary<string, ITableNGlycanProxy>();
            queue.Enqueue(root);
            List<IGlycan> glycans = new List<IGlycan>();
            while (queue.Count > 0)
            {
                ITableNGlycanProxy node = queue.Dequeue();
                foreach (MonosaccharideType suger in Enum.GetValues(typeof(MonosaccharideType)))
                {
                    List<ITableNGlycan> neighbors = node.Growth(suger);
                    foreach (ITableNGlycan n in neighbors)
                    {
                        string id = n.GetName();
                        try
                        {
                            if (generator.Criteria(n))
                            {

                                if (!visited.ContainsKey(id))
                                {
                                    ITableNGlycanProxy proxy = generator.Generate(n);
                                    queue.Enqueue(proxy);
                                    visited.Add(id, proxy);
                                    glycans.Add(proxy);
                                }
                                generator.Update(visited[id], node);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Exception occurs at " + id);
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }
            return glycans;
        }
    }
}
