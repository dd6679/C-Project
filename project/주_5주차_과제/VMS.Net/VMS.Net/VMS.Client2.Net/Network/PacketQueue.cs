using System.Collections.Generic;

namespace VMS.Client2.Net
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
}
