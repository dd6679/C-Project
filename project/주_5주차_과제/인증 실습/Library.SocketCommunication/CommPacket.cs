using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Library.SocketCommunication
{
    public class CommPacket
    {
        public CommHead Head { get; set; }
        public byte[] Data { get; set; }
        public object sender { get; set; }

        public static void FromArray(ref CommHead src, byte[] dst, int from)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((int)src.AllocSize)), 0, dst, from, 4);
            from += 4;
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)src.Command)), 0, dst, from, 2);
            from += 2;
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)src.Value)), 0, dst, from, 2);
            from += 2;
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((int)src.DataSize)), 0, dst, from, 4);
            from += 4;
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)src.RefIndex)), 0, dst, from, 2);
            from += 2;
            src.dataType = dst[from];
            from += 1;
            src.version = dst[from];
            from += 1;
            Buffer.BlockCopy(src.Nonce, 0, dst, from, 16);
            from += 16;

/*            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(src.srcTran)), 0, dst, from, 8);
            from += 8;
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(src.dstTran)), 0, dst, from, 8);
            from += 8;*/
        }

        public static int ToArray(byte[] src, int from, ref CommHead dst)
        {
            dst.AllocSize = (uint)IPAddress.NetworkToHostOrder((int)BitConverter.ToUInt32(src, from));
            from += 4;
            dst.Command = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(src, from));
            from += 2;
            dst.Value = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(src, from));
            from += 2;
            dst.DataSize = (uint)IPAddress.NetworkToHostOrder((int)BitConverter.ToUInt32(src, from));
            from += 4;
            dst.RefIndex = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(src, from));
            from += 2;
            dst.dataType = src[from];
            from += 1;
            dst.version = src[from];
            from += 1;
            Buffer.BlockCopy(src, 0, dst.Nonce, from, 16);
            from += 16;

            return from;
        }
    }
}
