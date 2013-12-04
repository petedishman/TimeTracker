using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.UI
{
    public class MainWindowViewModelFactory : IMainWindowViewModelFactory
    {
        public MainWindowViewModelFactory()
        {
            // any other dependencies that MainWindowViewModel has should be passed in here
            // ready for the create call
        }

        #region IViewModelFactory Members

        public MainWindowViewModel Create(IWindow window)
        {
            if (window == null)
            {
                throw new ArgumentNullException("window");
            }

            return new MainWindowViewModel(window);
        }

        #endregion
    }
}
