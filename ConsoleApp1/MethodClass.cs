using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class MethodClass
    {
        public static Task<string> Method1(string url)
        {
            WebRequest req = WebRequest.Create(url);

            IAsyncResult ar = req.BeginGetResponse((a) => Console.Write(a.IsCompleted), null);

            Task<string> downloadTask = Task.Factory.FromAsync<string>(ar, iar =>
            {
                using (WebResponse res = req.EndGetResponse(iar))
                {
                    using (var reader = new StreamReader(res.GetResponseStream()))
                    {
                        return reader.ReadToEnd();
                    }
                }
            });

            return downloadTask;
        }

        public static Task<string> Method3(string url)
        {
            WebRequest req = WebRequest.Create(url);
            IAsyncResult ar = req.BeginGetResponse(null, null);

            Task<string> downloadTask = Task.Factory.FromAsync<string>(ar, iar =>
            {
                using (WebResponse res = req.EndGetResponse(iar))
                {
                    using (var reader = new StreamReader(res.GetResponseStream()))
                    {
                        return reader.ReadToEnd();
                    }
                }
            });

            throw new Exception("Testing exceptions");
            return downloadTask;
            //Console.WriteLine("Downloaded T3");
        }

        public static async Task<long> Method4(int number, CancellationToken ct)
        {
            int i = 1;
            long s = 0;
            ct.Register(() => Console.WriteLine($"Method cancelled on {i} of {number}, at sum {s}"));
            //Console.WriteLine(number);
            for (i = 1; i <= number; i++)
            {

                ct.ThrowIfCancellationRequested();
                s += i;
            }
            //Console.WriteLine(number);
            //Console.WriteLine("Summed up T4");
            return s;
        }

        public static long Method8(int number, long start, CancellationToken ct)
        {
            int i = 1;
            long s = start;
            ct.Register(() => Console.WriteLine($"Method cancelled on {i} of {number}, at sum {s}"));
            //Console.WriteLine(number);
            for (i = 1; i <= number; i++)
            {

                ct.ThrowIfCancellationRequested();
                s += i;
            }
            //Console.WriteLine(number);
            //Console.WriteLine("Summed up T4");
            return s;
        }

        public static async Task<string> Method5(string ident, string filename, CancellationToken ct)
        {
            bool lockTaken = false;

            string result = "reader failed, timed-out";
            try
            {
                Monitor.TryEnter(filename, new TimeSpan(500), ref lockTaken);
                if (lockTaken)
                {
                    Console.WriteLine(ident + " entered lock");
                    ct.ThrowIfCancellationRequested();
                    result = File.ReadAllText(filename);
                    Thread.Sleep(20);
                    Console.WriteLine(ident + " left lock");
                }
                else
                {
                    throw new TimeoutException("Failed to get lock on: " + filename);
                }
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(filename);
            }
            return ident + ": " + result;

        }

        public static async Task<string> Method6(string ident, string filename, CancellationToken ct)
        {
            bool lockTaken = false;

            string result = "writer failed, timed-out";

            try
            {
                Monitor.TryEnter(filename, new TimeSpan(500), ref lockTaken);
                if (lockTaken)
                {
                    Console.WriteLine(ident + " entered lock");
                    ct.ThrowIfCancellationRequested();
                    File.WriteAllText(filename, "Now it says something else lol");
                    result = "Written some stuff";
                    Thread.Sleep(20);
                    Console.WriteLine(ident + " left lock");
                }
                else
                {
                    throw new TimeoutException("Failed to get lock on: " + filename);
                }
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(filename);
            }
            return ident + ": " + result;

        }

        public static async Task<string> Method7(CancellationToken ct, IProgress<ProgressImplementation> progressObserver)
        {
            progressObserver.Report(new ProgressImplementation(20));
            await Task.Delay(500);
            ct.ThrowIfCancellationRequested();
            progressObserver.Report(new ProgressImplementation(40));
            await Task.Delay(500);
            ct.ThrowIfCancellationRequested();
            progressObserver.Report(new ProgressImplementation(60));
            await Task.Delay(500);
            ct.ThrowIfCancellationRequested();
            progressObserver.Report(new ProgressImplementation(80));
            await Task.Delay(500);
            ct.ThrowIfCancellationRequested();
            progressObserver.Report(new ProgressImplementation(100));

            return "Successful";
        }

        public static string Method9(CancellationToken ct)
        {
            Console.WriteLine("Written in method9");
            for (int i = 0; i < 1000000000; i++) ;//Waste time, without calling delay

            Task child = Task.Factory.StartNew(() => throw new Exception("Another exception test, should propagate?"), TaskCreationOptions.AttachedToParent);
            Task child2 = Task.Factory.StartNew(() => throw new Exception("Another nother exception test, should propagate?"), TaskCreationOptions.AttachedToParent);

            return "Method9 yes";
        }

        public static void UnsafeIncrement(ref int n)
        {
            for (int i = 0; i < 50_000_000; i++)
            {
                n++;
            }
        }

        public static void SafeIncrement(ref int n)
        {
            for (int i = 0; i < 50_000_000; i++)
            {
                Interlocked.Add(ref n, 1);
            }
        }


        public static void DisplayProgress(ProgressImplementation progress)
        {
            //Console.SetCursorPosition(0,0); 
            Console.Write("Progress Tracker says: {0}% Done", progress.OverallProgress);
        }

    }
}
