using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.Core.DataCollection
{
    public interface IEqualish<T>
    {
        bool IsEqualishTo(T other);
    }

    public class UserActivity<T> where T : IEqualish<T>
    {
        public bool WasActive { get; set; }
        public int Seconds { get; set; }
        public T Details { get; set; }
    }
}
