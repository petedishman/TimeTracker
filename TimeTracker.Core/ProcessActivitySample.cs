using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.Core
{
    public class ProcessActivitySample
    {
        public int Seconds { get; set; }
        public bool WasActive { get; set; }
        public ProcessInfo ProcessInfo { get; set; }
    }
}
