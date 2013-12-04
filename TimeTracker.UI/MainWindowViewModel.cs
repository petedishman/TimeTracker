using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.UI
{
    public class MainWindowViewModel
    {
        private readonly IWindow window;

        public MainWindowViewModel(IWindow window)
        {
            this.window = window;
        }
    }
}
