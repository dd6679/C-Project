using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.SocketCommunication
{
    public class StackQueue
    {
        public Queue<string> logs;
        private object lockObject = new object();

        public StackQueue()
        {
            logs = new Queue<string>();
        }

        public void Add(string data)
        {
            lock (lockObject)
            {
                logs.Enqueue(data);
            }
        }

        public string Pop()
        {
            lock (lockObject)
            {
                return logs.Dequeue();
            }
        }
    }

    public class ThreadLoop
    {
        public static Thread _thread;
        protected static bool _isRunning;
        protected static ManualResetEvent manualEvent = new ManualResetEvent(false);

        public ThreadLoop()
        {
            _isRunning = true;
            _thread = new Thread(new ThreadStart(Run));
            _thread.Start();
        }

        public virtual void Run()
        {

        }
    }

    public class QueueService : ThreadLoop
    {
        private StackQueue stackQueue;

        public QueueService()
        {
            stackQueue = new StackQueue();
        }

        public string Queuing(string data)
        {
            stackQueue.Add(data);

            return stackQueue.Pop();
        }
    }
}
