using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SocketCommunication.Network
{
    // 패킷핸들러라는 델리게이트 선언
    public delegate void PacketHandler(Packet packet);
    public class PacketProcessor<T> : Dictionary<T, PacketHandler>
    {
        // 딕셔너리로 enum과 델리게이트 묶어줌
        public void Regist(T key, PacketHandler handler)
        {
            if (!ContainsKey(key))
            {
                Add(key, handler);
            }
        }

        // 딕셔너리로 묶여있는 델리게이트 찾아 method에 넣어줌
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
