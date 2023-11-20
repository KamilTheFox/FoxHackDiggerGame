using System;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.ExceptionServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Il2CppSystem;
using Il2CppSystem.Threading;
using IntPtr = System.IntPtr;
using Delegate = Il2CppSystem.Delegate;

namespace ThreadPool
{
    public class Thread : Il2CppSystem.Object, System.IDisposable
    {
        private System.Action action;

        private Il2CppSystem.Threading.Thread thisThread;
        public Thread(System.Action _action)
        {
            action = _action;

            Delegate @delegate = new Delegate(this, "StartThread");

            ThreadStart st = new Il2CppSystem.Threading.ThreadStart(this, @delegate.Pointer);

            thisThread = new Il2CppSystem.Threading.Thread(st);

            thisThread.IsBackground = true;

        }

        public void Dispose()
        {
            Start();
        }

        public void Start()
        {
            thisThread.Start();
        }
        private void StartThread()
        {
            action.Invoke();
        }
    }
}
