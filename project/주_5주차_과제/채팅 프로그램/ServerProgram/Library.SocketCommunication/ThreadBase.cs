using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.SocketCommunication
{
    public class ThreadBase
    {
        public Thread _thread;
        private ManualResetEvent _event = new ManualResetEvent(false);
        public bool _isRunning;

        public void Start()
        {
            _isRunning = true;

            _thread = new Thread(new ThreadStart(Run));
            _thread.Start();
        }

        public void ParameterizedStart(ThreadBase session, object server)
        {
            session._isRunning = true;

            session._thread = new Thread(new ParameterizedThreadStart(session.RunObject));
            session._thread.Start(server);
        }

        public virtual void Run()
        {
            while (_isRunning)
            {
                _event.Reset();
                _event.WaitOne();
            }
        }

        public virtual void RunObject(object o)
        {
            while (_isRunning)
            {
                _event.Reset();
                _event.WaitOne();
            }
        }

        #region Close // 종료시 호출
        protected void Close()
        {
            _isRunning = false;
            _event.Set();
            _thread.Join();
        }
        #endregion
    }
}
