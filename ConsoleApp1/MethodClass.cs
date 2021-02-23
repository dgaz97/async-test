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
            //Console.WriteLine(number);
            long s = 0;
            for (int i = 1; i <= number; i++)
            {
                if (ct.IsCancellationRequested)
                {
                    Console.WriteLine($"Method 4 cancelled at {i} out of {number} with sum {s}");
                    return s;
                }
                s += i;
            }
            //Console.WriteLine(number);
            //Console.WriteLine("Summed up T4");
            return s;
        }


    }
}
