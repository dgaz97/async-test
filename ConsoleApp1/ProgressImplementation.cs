using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class ProgressImplementation
    {
        public int OverallProgress { get; private set; }
        public ProgressImplementation(int overallProgress)
        {
            OverallProgress = overallProgress;
        }
    }
}
