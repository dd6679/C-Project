using System.Threading;

namespace VMS.Client2.Net
{
    public class ThreadLoop
    {
        private Thread _thread;
        private ManualResetEvent _mrEvent = new ManualResetEvent(false);

        public virtual void Start()
        {
            _thread = new Thread(new ThreadStart(RunInternal));
            _thread.Start();
        }

        public virtual void Close()
        {
            _mrEvent.Set();
            _thread.Join();
        }

        private void RunInternal()
        {
            Initialize();
            while (IsLoopRun)
            {
                if (IsThreadPause)
                {
                    _mrEvent.Reset();
                    _mrEvent.WaitOne();
                }
                else
                {
                    if (!Run())
                    {
                        break;
                    }
                }
            }
            Exit();
        }

        public virtual bool IsLoopRun
        {
            get
            {
                return false;
            }
        }

        public virtual bool IsThreadPause
        {
            get
            {
                return false;
            }
        }

        public virtual bool Run() { return false; }
        public virtual void Initialize() { }
        public virtual void Exit() { }
        public void Resum() { _mrEvent.Set(); }
    }
}
