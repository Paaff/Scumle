﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scumle.Helpers
{
    /// <summary>
    /// Abstract class to model the notion of a thread. Inherit this class
    /// and implement the RunThread() method for a concrete implementation
    /// </summary>
    abstract class BaseThread
    {
        private Thread _thread;

        protected BaseThread() { _thread = new Thread(new ThreadStart(this.RunThread)); }

        public void Start() { _thread.Start(); }

        // Override in base class
        protected abstract void RunThread();
    }
}
