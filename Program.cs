using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ParallelNet.SpeedTest;

namespace ParallelNet
{
    class Program
    {

        static void Main(string[] args)
        {
            new Market();
            
            new SpeedTest.SpeedTest();
        }
    }
}
