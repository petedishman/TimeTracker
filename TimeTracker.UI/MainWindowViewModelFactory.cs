using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Core.Schema.Services;

namespace TimeTracker.UI
{
    class MainWindowViewModelFactory : IMainWindowViewModelFactory
    {
        private IDataContext dataContext;

        public MainWindowViewModelFactory(IDataContext dataContext)
        {
            // any other dependencies that MainWindowViewModel has should be passed in here
            // ready for the create call
            this.dataContext = dataContext;
        }

        #region IViewModelFactory Members

        public MainWindowViewModel Create(IWindow window)
        {
            if (window == null)
            {
                throw new ArgumentNullException("window");
            }

            return new MainWindowViewModel(window, dataContext);
        }

        #endregion
    }
}
