using System;

namespace VMS.Client2.Net
{
    public class QueueThread : ThreadLoop
    {
        private bool _isRunning;
        private PacketQueue _queue = new PacketQueue();
        private NetworkBase _network;
        public QueueThread(NetworkBase network)
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

            if (_network.Send(packet.Data, 0, (int)packet.Header.AllocSize) == packet.Header.AllocSize)
            {
                Console.WriteLine($"Sent: {packet.Header.Command}");
            }

            return true;
        }

        public override void Exit()
        {
            Console.WriteLine("Queue Thread Loop Complete");
        }
    }
}
