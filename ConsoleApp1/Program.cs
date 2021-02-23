using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTest
{
    class Program
    {

        /*async static Task Main()
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
        }*/

        async static Task Main(string[] args)
        {
            Random r = new Random();
            Task<string> t1 = Task.Run<string>(MethodClass.Method1);
            Task<string> t2 = Task.Run<string>(MethodClass.Method2);
            Task<string> t3 = Task.Run<string>(MethodClass.Method3);
            Task<string> t4 = Task.Run<string>(() => MethodClass.Method4(r.Next(100_000_000, 1_000_000_000)));
            Task all = Task.WhenAll(t1, t2, t3, t4);
            
            try
            {
                Console.WriteLine("Starting download");
                while (!all.IsCompleted)
                {
                    Console.Write(".");
                    Thread.Sleep(250);
                }
                Console.WriteLine();

                //await all;
            }
            catch
            {
                AggregateException aggregateException = all.Exception;
                foreach (var e in aggregateException.InnerExceptions)
                {
                    Console.WriteLine(e.Message);
                }
            }
            finally
            {
                Console.WriteLine("All done");
                await Task.Delay(2222);

                if (t1.Exception == null)
                    Console.WriteLine(t1.Result.Length);
                if (t2.Exception == null)
                    Console.WriteLine(t2.Result.Length);
                if (t3.Exception == null)
                    Console.WriteLine(t3.Result.Length);
                else
                {
                    foreach (var i in t3.Exception.InnerExceptions)
                    {
                        Console.WriteLine(i.Message);
                    }
                }
                if (t4.Exception == null)
                    Console.WriteLine(t4.Result);
            }
            Console.ReadKey();

        }

    }
}
