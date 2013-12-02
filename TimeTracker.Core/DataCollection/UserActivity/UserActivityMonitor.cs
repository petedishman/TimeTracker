﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TimeTracker.Core.DataCollection.UserActivity
{
    public interface IUserActivityMonitor
    {
        DateTime LastUserActivityAt { get; }
        int SecondsSinceLastUserActivity { get; }
    }

    public class UserActivityMonitor : IUserActivityMonitor
    {
        private int idleForXSeconds = 0;

        private const int UserActivityCheckIntervalMilliSeconds = 1000;
        private Timer activityCheckTimer;

        public UserActivityMonitor()
        {
            SetUpTimer();
        }

        private void SetUpTimer()
        {
            TimerCallback callback = OnUserActivityCheckTimer;
            activityCheckTimer = new Timer(callback, null, 0, UserActivityCheckIntervalMilliSeconds);
        }

        private void OnUserActivityCheckTimer(object info)
        {
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;

            int systemUptime = Environment.TickCount;
            int lastInputTicks = 0;
            int idleTicks = 0;

            if (GetLastInputInfo(ref lastInputInfo))
            {
                lastInputTicks = (int)lastInputInfo.dwTime;
                idleTicks = systemUptime - lastInputTicks;

                idleForXSeconds = idleTicks / 1000;
            }
        }

        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        // Struct we'll need to pass to the function
        internal struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }

        public DateTime LastUserActivityAt
        {
            get 
            {
                return DateTime.Now.Subtract(TimeSpan.FromSeconds((double)idleForXSeconds));
            }
        }

        public int SecondsSinceLastUserActivity
        {
            get 
            {
                return idleForXSeconds; 
            }
        }
    }
}