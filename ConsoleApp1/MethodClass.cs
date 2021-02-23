using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class MethodClass
    {
        public static async Task<string> Method1()
        {
            WebRequest req = WebRequest.Create("https://en.wikipedia.org/wiki/Async/await");
            WebResponse res = req.GetResponse();
            var reader = new StreamReader(res.GetResponseStream());
            //Console.WriteLine("Downloaded T1");
            return reader.ReadToEnd();
        }

        public static async Task<string> Method2()
        {
            WebRequest req = WebRequest.Create("https://en.wikipedia.org/wiki/Python_(programming_language)");
            WebResponse res = req.GetResponse();
            var reader = new StreamReader(res.GetResponseStream());
            //Console.WriteLine("Downloaded T2");
            return reader.ReadToEnd();
        }

        public static async Task<string> Method3()
        {
            WebRequest req = WebRequest.Create("https://en.wikipedia.org/wiki/Python_(programming_language)");
            WebResponse res = req.GetResponse();
            var reader = new StreamReader(res.GetResponseStream());
            throw new Exception("Testing exceptions");
            //Console.WriteLine("Downloaded T3");
            return reader.ReadToEnd();
        }

        public static async Task<string> Method4(int number)
        {
            //Console.WriteLine(number);
            long s = 0;
            for (int i = 1; i <= number; i++)
            {
                s += i;
            }
            //Console.WriteLine(number);
            //Console.WriteLine("Summed up T4");
            return s.ToString();
        }

    }
}
