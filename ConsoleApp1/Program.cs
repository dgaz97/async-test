using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread monitorThread = new Thread(new ThreadStart(MonitorNetwork));
            monitorThread.Start();

            Console.ReadKey();
        }

        static void MonitorNetwork()
        {
            long s = 0;
            for (int i = 1; i <= 5000000; i++)
            {
                s += i;
                if (i % 10000 == 0)
                {
                    Console.WriteLine(i);
                }
            }
            Console.WriteLine(s);
        }
    }
}
