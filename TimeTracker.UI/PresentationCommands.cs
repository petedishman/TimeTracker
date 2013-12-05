using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TimeTracker.UI
{
    public static class PresentationCommands
    {
        private readonly static RoutedCommand accept = new RoutedCommand("Accept", typeof(PresentationCommands));

        public static RoutedCommand Accept
        {
            get { return PresentationCommands.accept; }
        }
    }
}
