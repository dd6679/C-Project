using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.SocketCommunication
{
    public class PacketQueue : Queue<Packet>
    {
        private object _locker = new object();
        public void Add(Packet packet)
        {
            lock (_locker)
            {
                Enqueue(packet);
            }
        }

        public Packet Pop()
        {
            lock (_locker)
            {
                return Dequeue();
            }
        }

        public bool HasPacket
        {
            get
            {
                lock (_locker)
                {
                    return Count > 0;
                }
            }
        }

        public void ClearAll()
        {
            lock (_locker)
            {
                Clear();
            }
        }
    }
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
    class QueueService : ThreadLoop
    {
        private bool _isRunning;
        private PacketQueue _queue = new PacketQueue();
        private NetworkBase _network;
        public QueueService(NetworkBase network)
        {
            _network = network;
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Close()
        {
            _isRunning = false;
            _queue.ClearAll();

            base.Close();
        }

        public void Add(Packet packet)
        {
            _queue.Add(packet);
            Resum();
        }

        public override bool IsLoopRun
        {
            get
            {
                return _isRunning;
            }
        }

        public override bool IsThreadPause
        {
            get
            {
                return !_queue.HasPacket;
            }
        }
        public override void Initialize()
        {
            _isRunning = true;
        }

        public override bool Run()
        {
            var packet = _queue.Dequeue();

            if (_network.Send(packet.Data, 0, (int)packet.Head.AllocSize) == packet.Head.AllocSize)
            {
                Console.WriteLine($"Sent: {packet.Head.Command}");
            }

            return true;
        }

        public override void Exit()
        {
            Console.WriteLine("Queue Thread Loop Complete");
        }
    }
}
