﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.UI
{
    interface IMainWindowViewModelFactory
    {
        MainWindowViewModel Create(IWindow window);
    }
}
