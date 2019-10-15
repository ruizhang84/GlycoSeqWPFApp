using Autofac;
using GlycoSeqClassLibrary.Analyze;
using GlycoSeqClassLibrary.Builder.Spectrum;
using GlycoSeqClassLibrary.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqWPFApp
{
    public class MultiThreadSearch
    {
        Counter counter;
        IContainer container;
        IResults results;

        Queue<Tuple<int, int>> tasks;
        private readonly object queueLock = new object();
        private readonly object resultLock = new object();

        public MultiThreadSearch(Counter counter, IContainer container, IResults results)
        {
            this.results = results;
            this.counter = counter;
            this.container = container;
            
            tasks = new Queue<Tuple<int, int>>();
            GenerateTasks();
        }

        public Tuple<int, int> TryGetTask()
        {
            lock (queueLock)
            {
                if (tasks.Count > 0)
                    return tasks.Dequeue();
                return null;
            }
        }

        public void UpdateTask(IResults result)
        {
            lock (resultLock)
            {
                results.Merge(result);
            }
        }

        public void Run()
        {
            List<Task> searches = new List<Task>();
            for (int i = 0; i < SearchParameters.Access.ThreadNums; i++)
            {
                searches.Add(Task.Run(() => Search()));
            }

            Task.WaitAll(searches.ToArray());
        }

        private void GenerateTasks()
        {
            using (var scope = container.BeginLifetimeScope())
            {
                ISpectrumFactory spectrumFactory = scope.Resolve<ISpectrumFactory>();
                spectrumFactory.Init(SearchParameters.Access.MSMSFile);
                int start = spectrumFactory.GetFirstScan();
                int end = spectrumFactory.GetLastScan();

                List<int> msSpectrumScans = new List<int>();
                for (int scan = start; scan <= end; scan++)
                {
                    if (spectrumFactory.GetMSnOrder(scan) == 1)
                    {
                        msSpectrumScans.Add(scan);
                    }
                }

                for (int i = 0; i < msSpectrumScans.Count; i++)
                {
                    if (i < msSpectrumScans.Count - 1)
                    {
                        tasks.Enqueue(new Tuple<int, int>(msSpectrumScans[i], msSpectrumScans[i + 1] - 1));
                    }
                    else
                    {
                        tasks.Enqueue(new Tuple<int, int>(msSpectrumScans[i], end));
                    }
                }
            }
        }

        private void Search()
        {
            using (var scope = container.BeginLifetimeScope())
            {
                Tuple<int, int> scanPair;
                ISearchEngine searchEngine = scope.Resolve<ISearchEngine>();
                searchEngine.Init(
                    SearchParameters.Access.MSMSFile, 
                    SearchParameters.Access.FastaFile, 
                    SearchParameters.Access.OutputFile);
                progress send = new progress((scan) => counter.Add(scan));

                while ((scanPair = TryGetTask()) != null)
                {
                    searchEngine.Search(scanPair.Item1, scanPair.Item2, send);
                }

                UpdateTask(searchEngine.GetResults());
            }
        }
    }
}
