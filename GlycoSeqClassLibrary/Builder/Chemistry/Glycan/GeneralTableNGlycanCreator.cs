using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlycoSeqClassLibrary.Model.Chemistry.Glycan;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Glycan
{
    public class GeneralTableNGlycanCreator : IGlycanCreator
    {
        ITableNGlycanProxyGenerator generator;
        TableNGlycanProxyTemplate root;

        public GeneralTableNGlycanCreator(ITableNGlycanProxyGenerator generator, TableNGlycanProxyTemplate glycan)
        {
            this.generator = generator;
            root = glycan;
        }

        public List<IGlycan> Create()
        {
            
            Queue<TableNGlycanProxyTemplate> queue = new Queue<TableNGlycanProxyTemplate>();
            Dictionary<string, TableNGlycanProxyTemplate> visited = new Dictionary<string, TableNGlycanProxyTemplate>();
            queue.Enqueue(root);
            List<IGlycan> glycans = new List<IGlycan>();
            while (queue.Count > 0)
            {
                TableNGlycanProxyTemplate node = queue.Dequeue();
                foreach (MonosaccharideType suger in Enum.GetValues(typeof(MonosaccharideType)))
                {
                    List<ITableNGlycan> neighbors = node.Growth(suger);
                    foreach (ITableNGlycan n in neighbors)
                    {
                        string id = n.GetName();
                        if (generator.Criteria(n))
                        {
                            if (!visited.ContainsKey(id))
                            {
                                TableNGlycanProxyTemplate proxy = generator.Generate(n);
                                queue.Enqueue(proxy);
                                visited.Add(id, proxy);
                                glycans.Add(proxy);
                            }
                            generator.Update(visited[id], node);

                        }
                    }
                }
            }
            return glycans;
        }
    }
}
