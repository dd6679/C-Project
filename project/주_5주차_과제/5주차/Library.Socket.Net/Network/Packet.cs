using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Library.SocketCommunication
{
    public class Packet
    {
        public object sender;
        public PacketHeader Head;
        public byte[] Data;

        public static int CopyHeader(byte[] src, int from, ref Packet dst)
        {
            Debug.Assert(src.Length >= from + PacketHeader.CommHeadSize);

            int newPos = PacketHeader.FromArray(src, from, ref dst.Head);
            Debug.Assert(newPos - from == PacketHeader.CommHeadSize);

            // AllocSize 유효성 검사
            if ((dst.Head.AllocSize % 16) != 0 ||
                dst.Head.DataSize > dst.Head.AllocSize ||
                dst.Head.AllocSize - 15 < dst.Head.DataSize ||
                (dst.Head.version != 0 && dst.Head.version != 1))
            {
                return -1;
            }

            dst.Data = new byte[dst.Head.AllocSize];
            Buffer.BlockCopy(src, from, dst.Data, 0, PacketHeader.CommHeadSize);

            return newPos;
        }

        public static Packet MakePacket(ushort command, ushort value, string msg, ushort refIndex, CommDataTypes dataType, long srcTran = -1, long dstTran = -1)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            var packet = Initialize(command, value, data.Length, refIndex, dataType, srcTran, dstTran);
            UpdateData(ref packet, data, 0, data.Length);

            return packet;
        }

        public static Packet Initialize(ushort command, ushort value, int len, ushort refIndex, CommDataTypes dataType, long srcTran = -1, long dstTran = -1)
        {
            var packet = new Packet();
            packet.Head = PacketHeader.Initialize(command, value, (uint)len, refIndex, dataType, srcTran, dstTran);
            packet.Data = new byte[packet.Head.AllocSize];
            return packet;
        }

        public static void UpdateData(ref Packet packet, byte[] data, int from, int length)
        {
            Buffer.BlockCopy(data, (int)from, packet.Data, (int)(PacketHeader.CommHeadSize), (int)length);
        }
    }
}
