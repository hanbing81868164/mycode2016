using System.Threading;
using System.Windows.Forms;

namespace System
{
    public delegate void ThreadFunctionDelegate();
    public delegate void CallBackDelegate(string status);
    public class CallBackThread : IDisposable
    {
        private Control m_BaseControl;
        private ThreadFunctionDelegate m_ThreadFunction;
        private CallBackDelegate m_CallBackFunction;
        private Thread m_Thread;
        private bool m_disposedValue;
        private bool m_startedValue;

        public CallBackThread(ref Control caller, ref ThreadFunctionDelegate threadMethod, ref CallBackDelegate callbackFunction)
        {
            this.m_disposedValue = false;
            this.m_startedValue = false;
            this.m_BaseControl = caller;
            this.m_ThreadFunction = threadMethod;
            this.m_CallBackFunction = callbackFunction;
            this.m_Thread = new Thread(new ThreadStart(this.ThreadFunction));
        }
        public void Start()
        {
            bool flag = !this.m_startedValue;
            if (flag)
            {
                if (this.m_Thread != null)
                    this.m_Thread.Start();
                this.m_startedValue = true;
                return;
            }
            throw new Exception("Thread already started");
        }
        private void ThreadFunction()
        {
            this.m_ThreadFunction();
            this.m_startedValue = false;
        }
        public void UpdateUI(string msg)
        {
            bool flag = this.m_BaseControl != null && this.m_CallBackFunction != null;
            if (flag)
            {
                this.m_BaseControl.Invoke(this.m_CallBackFunction, new object[]
				{
					msg
				});
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            bool flag = !this.m_disposedValue;
            if (flag)
            {
                if (disposing && this.m_Thread != null)
                {
                    bool flag2 = this.m_Thread.ThreadState != ThreadState.Stopped;
                    if (flag2)
                    {
                        this.m_Thread.Abort();
                    }
                }
                this.m_Thread = null;
                this.m_BaseControl = null;
                this.m_CallBackFunction = null;
            }
            this.m_disposedValue = true;
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
