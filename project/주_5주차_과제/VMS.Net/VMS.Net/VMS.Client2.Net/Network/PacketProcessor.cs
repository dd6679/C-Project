using System.Collections.Generic;

namespace VMS.Client2.Net
{
    public delegate void PacketHandler(Packet packet);
    public class PacketProcessor<T> : Dictionary<T, PacketHandler>
    {
        public void Regist(T key, PacketHandler handler)
        {
            if (!ContainsKey(key))
            {
                Add(key, handler);
            }
        }

        public PacketHandler Resolve(T key)
        {
            PacketHandler method = null;
            if (ContainsKey(key))
            {
                method = this[key];
            }
            return method;
        }
    }
}
