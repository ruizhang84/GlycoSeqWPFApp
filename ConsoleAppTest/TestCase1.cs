using GlycoSeqClassLibrary.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    public class MassPoint : IPoint
    {
        public double Value { get; set; }

        public MassPoint(double value)
        {
            Value = value;
        }

        public int CompareTo(IPoint other)
        {
            return (int) Math.Ceiling(Value - other.GetValue());
        }

        public double GetValue()
        {
            return Value;
        }
    }

    public class TestCase1 : TestCase
    {
        public void Run()
        {
            List<IPoint> points = new List<IPoint>();
            for (int i = 0; i < 1200; i++)
            {
                points.Add(new MassPoint(i));
            }

            
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            IComparer<IPoint> comparer = new ToleranceComparer(1.1);

            Console.WriteLine(comparer.Compare(new MassPoint(122), new MassPoint(123)));
            BinarySearch bins = new BinarySearch(comparer);
            bins.setData(points);
            List<IPoint> found = new List<IPoint>();
            for (int j = 122; j < 123; j += 10)
                found.AddRange(bins.Search(new MassPoint(j)));
            //BucketSearch bucket = new BucketSearch(points, 1.1);
            //List<IPoint> result = bucket.Search(new MassPoint(3));

            watch.Stop();
            Console.WriteLine(found.Count);
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

            found.Clear();
            watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            //BinarySearch bins = new BinarySearch(points, 1.1);
            //List<IPoint> result = bins.Search(new MassPoint(3));
            BucketSearch bucket = new BucketSearch(comparer, 1.1);
            bucket.setData(points);

            for (int j = 122; j < 123; j += 10)
                found.AddRange(bucket.Search(new MassPoint(j)));

            watch.Stop();
            Console.WriteLine(found.Count);
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");




            //foreach (IPoint p in result)
            //{
            //    Console.WriteLine(p.GetValue());
            //}

            Console.ReadLine();
        }
    }
}
