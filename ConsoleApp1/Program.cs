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
            var CancelTokenSource1 = new CancellationTokenSource();
            var CancelTokenSource2 = new CancellationTokenSource();
            var CancelTokenSource3 = new CancellationTokenSource();
            var CancelTokenSource4 = new CancellationTokenSource();
            var CancelTokenSource5 = new CancellationTokenSource();

            Mutex m = new Mutex(false, "mut");
            //m.
            Progress<ProgressImplementation> po = new Progress<ProgressImplementation>(MethodClass.DisplayProgress);

            Random r = new Random();
            Task<string> t1 = Task.Run(() => MethodClass.Method1("https://en.wikipedia.org/wiki/Async/await"));
            Task<string> t2 = Task.Run(() => MethodClass.Method1("https://en.wikipedia.org/wiki/Python_(programming_language)"));
            Task<string> t3 = Task.Run(() => MethodClass.Method3("https://en.wikipedia.org/wiki/Futures_and_promises"));
            ///Task<long> t4 = Task.Run(() => MethodClass.Method4(r.Next(1_000_000_000, 2_000_000_000), CancelTokenSource1.Token));
            ///Task<long> t5 = Task.Run(() => MethodClass.Method4(r.Next(1_000_000_000, 2_000_000_000), CancelTokenSource2.Token));
            ///Task<long> t6 = t5.ContinueWith(tres => MethodClass.Method8(r.Next(1_000_000_000, 2_000_000_000),tres.Result, CancelTokenSource3.Token));

            Task<string> t7 = Task.Run(() => MethodClass.Method5("ident 1", @"C:\test\test.txt", m, CancelTokenSource4.Token));
            Task<string> t8 = Task.Run(() => MethodClass.Method6("ident 2", @"C:\test\test.txt", m, CancelTokenSource4.Token));
            Task<string> t9 = Task.Run(() => MethodClass.Method5("ident 3", @"C:\test\test.txt", m, CancelTokenSource4.Token));

            Task<string> t10 = Task.Run(() => MethodClass.Method7(CancelTokenSource5.Token, po));

            Task<string> t11 = Task.Factory.StartNew(() => MethodClass.Method9(CancelTokenSource5.Token));

            Task all = Task.WhenAll(t1, t2, t3/*, t4,t5,t6*/, t7, t8, t9, t10, t11);
            try
            {
                //CancelTokenSource3.CancelAfter(2000);
                CancelTokenSource4.CancelAfter(100);



                Console.WriteLine("Starting download");
                while (!all.IsCompleted)
                {
                    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
                    {
                        CancelTokenSource1.Cancel();
                        //Method4CancelTokenSource2.Cancel();
                    }

                    Console.Write(".");
                    await Task.Delay(500);
                }
                Console.WriteLine();
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
                Console.WriteLine("All done, preparing results");
                await Task.Delay(1000);

                if (t1.Exception == null)
                    Console.WriteLine(t1.Result.Length);
                if (t2.Exception == null)
                    Console.WriteLine(t2.Result.Length);
                if (t3.Exception == null)
                    Console.WriteLine(t3.Result.Length);
                else
                {
                    foreach (var e in t3.Exception.InnerExceptions)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                //if (t4.Exception == null && t4.Status!=TaskStatus.Canceled)
                //    Console.WriteLine(t4.Result);
                //else if(t4.Status != TaskStatus.Canceled)
                //{
                //    foreach (var e in t4.Exception.InnerExceptions)
                //    {
                //        Console.WriteLine(e.Message);
                //    }
                //}else
                //    Console.WriteLine(t4.Status);
                //
                //if (t5.Exception == null && t5.Status != TaskStatus.Canceled)
                //    Console.WriteLine(t5.Result);
                //else if(t5.Status != TaskStatus.Canceled)
                //{
                //    foreach (var e in t5.Exception.InnerExceptions)
                //    {
                //        Console.WriteLine(e.Message);
                //    }
                //}
                //else
                //    Console.WriteLine(t5.Status);
                //
                //if (t6.Exception == null && t6.Status != TaskStatus.Canceled)
                //    Console.WriteLine(t6.Result);
                //else if(t6.Status != TaskStatus.Canceled)
                //{
                //    foreach (var e in t6.Exception.InnerExceptions)
                //    {
                //        Console.WriteLine(e.Message);
                //    }
                //}
                //else
                //    Console.WriteLine(t6.Status);

                if (t7.Status == TaskStatus.RanToCompletion)
                    Console.WriteLine(t7.Result);
                else
                    Console.WriteLine(t7.Status);
                if (t8.Status == TaskStatus.RanToCompletion)
                    Console.WriteLine(t8.Result);
                else
                    Console.WriteLine(t8.Status);
                if (t9.Status == TaskStatus.RanToCompletion)
                    Console.WriteLine(t9.Result);
                else
                    Console.WriteLine(t9.Status);

                if (t10.Status == TaskStatus.RanToCompletion)
                    Console.WriteLine(t10.Result);
                else
                    Console.WriteLine(t10.Status);

                if (t11.Exception == null)
                    Console.WriteLine(t11.Result.Length);
                else
                {
                    foreach (Exception e in t11.Exception.InnerExceptions)
                    {
                        Console.WriteLine(e.InnerException.Message);
                    }
                }


            }
            Console.ReadKey();

        }

    }
}
