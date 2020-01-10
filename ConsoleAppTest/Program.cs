using GlycoSeqClassLibrary.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    public interface TestCase
    {
        void Run();
    }

    class Program
    {
        static void Main(string[] args)
        {
            TestCase t = new TestCase13();
            t.Run();

        }       
       
    }
}
