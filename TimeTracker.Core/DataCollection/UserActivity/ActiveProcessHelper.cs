﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Core.Helpers;

namespace TimeTracker.Core.DataCollection.UserActivity
{
    public interface IActiveProcessHelper
    {
        /// <summary>
        /// Returns the currently active process
        /// </summary>
        /// <returns></returns>
        ProcessInfo GetActiveProcess();
    }


    public class ActiveProcessHelper : IActiveProcessHelper
    {
        public const string UnknownProcessTitle = "Unknown Process";
        public const string UnknownWindowTitle = "Unknown Window Title";

        public ProcessInfo GetActiveProcess()
        {
            var activeProcess = GetForegroundProcessFromWindowHandle(SafeNativeMethods.GetForegroundWindow());
            if (activeProcess != null)
            {
                return new ProcessInfo(activeProcess.ProcessName, activeProcess.MainWindowTitle);
            }
            else
            {
                return new ProcessInfo(UnknownProcessTitle, UnknownWindowTitle);
            }
        }

        private Process GetForegroundProcessFromWindowHandle(IntPtr foregroundWindowHandle)
        {
            try
            {
                uint processId = 0;
                if (SafeNativeMethods.GetWindowThreadProcessId(foregroundWindowHandle, out processId) > 0)
                {
                    return Process.GetProcessById((int)processId);
                }
            }
            catch (Exception e)
            {
                Debug.Write(String.Format("Caught exception getting active process: {0}", e.Message));
            }

            return null;
        }
    }
}
