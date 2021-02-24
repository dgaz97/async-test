using System.Threading;

namespace AsyncTest
{
    public class SumPair
    {
        public SumPair(int n)
        {
            numberToSum = n;
            Thread.Sleep(2000);
        }
        public int numberToSum { get; set; }
        public long sum { get; set; }
    }
}
