using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ParallelNet
{
    class Program
    {
        static IEnumerable<int> PrimeFinder(int from, int to)
        {
            IList<int> primeNumbs = new List<int>();
            for (int i = from; i < to; i++)
            {
                bool isPrime = true;
                for (int j = 2; j < i; j++)
                {
                    if (i % j == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }

                if (isPrime)
                {
                    primeNumbs.Add(i);
                }
            }

            return primeNumbs;
        }

        static bool PIsPrime(int number) => Parallel.For(2, number, (i, parallelLoopState) =>
            {
                if (number % i == 0) parallelLoopState.Break();
            }
        ).IsCompleted;
        
        static IEnumerable<int> PPrimeFinder(int from, int to)
        {
            IList<int> primeNumbs = new List<int>();
            Parallel.For(from, to, i => { if (PIsPrime(i)) primeNumbs.Add(i);});
            return primeNumbs;
        }

        
        static void Main(string[] args)
        {

            for (int i = 1; i < 100; i++)
            {
                Stopwatch sw = Stopwatch.StartNew();
                var prime = PrimeFinder(1, (int)Math.Pow(10, i));
                Console.WriteLine($"OO prime finder. Rng { (int)Math.Pow(10, i)}. Time {sw.Elapsed}"); //2:32

                Stopwatch swp = Stopwatch.StartNew();
                var pprime = PPrimeFinder(1, (int)Math.Pow(10, i));
                Console.WriteLine($"PW prime finder. Rng { (int)Math.Pow(10, i)}. Time {swp.Elapsed}"); //1:13
            }
        }
    }
}
