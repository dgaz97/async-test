using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTest
{
    class Program
    {

        public static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private async static Task MainAsync()
        {
            Dictionary<int, SumPair> toSum = new Dictionary<int, SumPair>();

            List<Task> threads = new List<Task>();

            Random r = new Random();

            var semaphore = new SemaphoreSlim(16);

            for (int i = 1; i <= 500; i++)//Generate 500 tasks
            {
                toSum.Add(i, new SumPair() { numberToSum = r.Next(10000000, 100000000) });//Between 10M and 100M
            }

            DateTime timeStart = DateTime.Now;
            foreach (KeyValuePair<int, SumPair> kvp in toSum)
            {
                Task t = Task.Factory.StartNew<KeyValuePair<int, SumPair>>(() =>
                {
                    Console.WriteLine($"{kvp.Key} -- {kvp.Value.numberToSum}");
                    semaphore.Wait();
                    long s = 0;
                    for (int i = 1; i <= kvp.Value.numberToSum; i++)
                    {
                        s += i;
                    }
                    kvp.Value.sum = s;
                    Console.WriteLine($"{kvp.Key} -- {s}");
                    semaphore.Release();
                    Console.WriteLine("Free semaphore_ " + semaphore.CurrentCount);
                    return kvp;
                });
                threads.Add(t);
            }


            await Task.WhenAll(threads);

            DateTime timeEnd = DateTime.Now;
            TimeSpan span = timeEnd - timeStart;
            Console.WriteLine($"Multithreaded time: {span.TotalSeconds}");

            timeStart = DateTime.Now;
            foreach (KeyValuePair<int, SumPair> kvp in toSum)
            {
                //var kvp = (KeyValuePair<int, SumPair>)o;
                Console.WriteLine($"{kvp.Key} -- {kvp.Value.numberToSum}");
            
                long s = 0;
                for (int i = 1; i <= kvp.Value.numberToSum; i++)
                {
                    s += i;
                }
                kvp.Value.sum = s;
                Console.WriteLine($"{kvp.Key} -- {s}");
                //return kvp;
            }
            timeEnd = DateTime.Now;
            span = timeEnd - timeStart;
            Console.WriteLine($"SingleThreaded time: {span.TotalSeconds}");


            foreach (Task<KeyValuePair<int, SumPair>> t in threads)
            {
                toSum[t.Result.Key] = t.Result.Value;
            }

            Console.WriteLine("SVI REZULTATI");
            foreach (KeyValuePair<int, SumPair> kvp in toSum)
            {
                Console.WriteLine($"{kvp.Key} --- {kvp.Value.numberToSum} --- {kvp.Value.sum}");
            }
            Console.WriteLine("All done");


            Console.ReadKey();
        }



    }
}
