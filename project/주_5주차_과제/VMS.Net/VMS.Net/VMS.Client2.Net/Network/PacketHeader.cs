using System;
using System.Net;

namespace VMS.Client2.Net
{
    public struct PacketHeader
    {
        public const int CommHeadNonceFrom = 16;
        public const int CommHeadNonceSize = 16;
        public const int CommHeadSize = 32;
        public const int CipherBlockSize = 16;
        public const byte COMM_VERSION = 1;

        public uint AllocSize;
        public ushort Command;
        public ushort Value;
        public uint DataSize;
        public ushort RefIndex;
        public byte dataType;
        public byte version;
        public byte[] Nonce;
        public long srcTran, dstTran;

        public static void Encode(ref PacketHeader src, byte[] dst, int from)
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
            dst[from++] = src.dataType;
            dst[from++] = src.version;

        }

        public static int Decode(byte[] src, int from, ref PacketHeader dst)
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
            dst.dataType = src[from++];
            dst.version = src[from++];

            dst.Nonce = new byte[CommHeadNonceSize];
            System.Diagnostics.Debug.Assert(dst.Nonce != null && dst.Nonce.Length == CommHeadNonceSize);
            System.Diagnostics.Debug.Assert(dst.DataSize <= dst.AllocSize);
            if (dst.version > 0)
            {
                dst.srcTran = (long)IPAddress.NetworkToHostOrder((long)BitConverter.ToUInt64(src, from));
                dst.dstTran = (long)IPAddress.NetworkToHostOrder((long)BitConverter.ToUInt64(src, from + 8));
            }
            Buffer.BlockCopy(src, from, dst.Nonce, 0, CommHeadNonceSize);
            from += CommHeadNonceSize;

            return from;
        }

        public static PacketHeader Initialize(ushort command, ushort value, uint size, ushort refIndex, CommDataTypes dataType, long srcTran, long dstTran)
        {
            var header = new PacketHeader();

            uint allocSize = size + CommHeadSize;
            header.AllocSize = allocSize + (allocSize % CipherBlockSize > 0 ? CipherBlockSize - (allocSize % CipherBlockSize) : 0);
            header.DataSize = size;
            header.Command = command;
            header.Value = value;
            header.RefIndex = refIndex;
            header.dataType = (byte)dataType;
            header.version = COMM_VERSION;
            header.Nonce = null;
            header.srcTran = srcTran;
            header.dstTran = dstTran;

            return header;
        }
    }
}
