using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelNet.SpeedTest
{
    public  class SpeedTest
    {
        public SpeedTest()
        {
            int max = 100000;
            int[] numbers = new int[max];
            for (int i = 0; i < max; i++) numbers[i] = i;

            Stopwatch stnp = Stopwatch.StartNew();
            int[] npres = new int[max];
            for (int i = 0; i < max; i++)
                npres[i] = Factorial(i);
            Console.WriteLine($"Скорость подсчета в 1 потоке {stnp.Elapsed}"); //14.5 s

            Console.WriteLine("============");

            Stopwatch stp = Stopwatch.StartNew();
            var pres = from n in numbers.AsParallel().AsOrdered() select Factorial(n);
            pres.ForAll(a => { int b = a; }); //Обманка, чтобы он все же посчитал каждое значение    
            Console.WriteLine($"Скорость подсчета в нескольких потоках {stp.Elapsed}"); //03.8 s

            Console.WriteLine("=======================");

            for (int i = 6; i < 100; i++)
            {
                Stopwatch sw = Stopwatch.StartNew();
                var prime = PrimeFinder(1, (int)Math.Pow(10, i));
                Console.WriteLine($"OO prime finder. Rng { (int)Math.Pow(10, i)}. Time {sw.Elapsed}"); //2:32 m

                Stopwatch swp = Stopwatch.StartNew();
                var pprime = PPrimeFinder(1, (int)Math.Pow(10, i));
                Console.WriteLine($"PW prime finder. Rng { (int)Math.Pow(10, i)}. Time {swp.Elapsed}"); //1:13 m
            }
        }


        private IEnumerable<int> PrimeFinder(int from, int to)
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

        private bool PIsPrime(int number) => Parallel.For(2, number, (i, parallelLoopState) =>
        {
            if (number % i == 0) parallelLoopState.Break();
        }
        ).IsCompleted;

        private IEnumerable<int> PPrimeFinder(int from, int to)
        {
            IList<int> primeNumbs = new List<int>();
            Parallel.For(from, to, i => { if (PIsPrime(i)) primeNumbs.Add(i); });
            return primeNumbs;
        }


        private int Factorial(int x)
        {
            int result = 1;
            for (int i = 1; i <= x; i++)
                result *= i;
            return result;
        }

       
    }
}
