using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Client.Net.Constance;

namespace Library.Client.Net.Network
{
    public struct Packet
    {
        public object sender;
        public PacketHeader Header;
        public byte[] Data;

        public static int CopyHeader(byte[] src, int from, ref Packet dst)
        {
            Debug.Assert(src.Length >= from + PacketHeader.CommHeadSize);

            int newPos = PacketHeader.Decode(src, from, ref dst.Header);
            Debug.Assert(newPos - from == PacketHeader.CommHeadSize);

            if ((dst.Header.AllocSize % 16) != 0 ||
                dst.Header.DataSize > dst.Header.AllocSize ||
                dst.Header.AllocSize - 15 < dst.Header.DataSize ||
                (dst.Header.version != 0 && dst.Header.version != 1))
            {
                Console.WriteLine("Invalid Alloc size!!");
                return -1;
            }

            dst.Data = new byte[dst.Header.AllocSize];
            Buffer.BlockCopy(src, from, dst.Data, 0, PacketHeader.CommHeadSize);

            return newPos;
        }

        public static Packet MakePaket(ushort command, ushort value, string msg, ushort refIndex, CommDataTypes dataType, long srcTran = -1, long dstTran = -1)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            var packet = Initialize(command, value, data.Length, refIndex, dataType, srcTran, dstTran);
            UpdateData(ref packet, data, 0, data.Length);

            return packet;
        }

        public static Packet Initialize(ushort command, ushort value, int len, ushort refIndex, CommDataTypes dataType, long srcTran = -1, long dstTran = -1)
        {
            var packet = new Packet();
            packet.Header = PacketHeader.Initialize(command, value, (uint)len, refIndex, dataType, srcTran, dstTran);
            packet.Data = new byte[packet.Header.AllocSize];
            return packet;
        }

        public static void UpdateData(ref Packet packet, byte[] data, int from, int length)
        {
            Buffer.BlockCopy(data, (int)from, packet.Data, (int)(PacketHeader.CommHeadSize), (int)length);
        }
    }
}