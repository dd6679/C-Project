using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Library.SocketCommunication
{
    [Serializable]
    public class PacketHeader
    {
        // 상수
        public const int CommHeadNonceFrom = 16;
        public const int CommHeadNonceSize = 16;
        public const int CommHeadSize = 32;
        public const int CipherBlockSize = 16;
        public const byte COMM_VERSION = 1;

        // 필드
        public uint AllocSize;
        public ushort Command;
        public ushort Value;
        public uint DataSize;
        public ushort RefIndex;
        public byte dataType;
        public byte version;
        public byte[] Nonce;
        public long srcTran, dstTran;

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

        // 패킷에서 배열 변환
        public static void ToArray(ref PacketHeader src, byte[] dst, int from)
        {
            // 패킷헤더 src 각 필드에서 배열 dst로 바이트 수 복사
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
            // 1바이트이므로
            dst[from++] = src.dataType;
            dst[from++] = src.version;
        }

        // 배열에서 패킷 변환
        public static int FromArray(byte[] src, int from, ref PacketHeader dst)
        {
            // 배열 src에서 패킷헤더 src 각 필드에 값 저장
            dst.AllocSize = (uint)IPAddress.NetworkToHostOrder((int)BitConverter.ToUInt32(src, from));
            from += 4;
            dst.Command = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(src, from));
            from += 2;
            dst.Value = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(src, from));
            from += 2;
            dst.DataSize = (uint)IPAddress.NetworkToHostOrder((int)BitConverter.ToInt32(src, from));
            from += 4;
            dst.RefIndex = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToInt16(src, from));
            from += 2;
            dst.dataType = src[from++];
            dst.version = src[from++];

            dst.Nonce = new byte[CommHeadNonceSize];
            
            // 변수들의 유효성 검사, 조건이 거짓일 때 메세지박스 출력, 여기서 오류나는 문제
            Debug.Assert(dst.Nonce != null && dst.Nonce.Length == CommHeadNonceSize);
            Debug.Assert(dst.DataSize <= dst.AllocSize);

            if (dst.version > 0)
            {
                dst.srcTran = (long)IPAddress.NetworkToHostOrder((long)BitConverter.ToUInt64(src, from));
                dst.dstTran = (long)IPAddress.NetworkToHostOrder((long)BitConverter.ToUInt64(src, from + 8));
            }
            Buffer.BlockCopy(src, from, dst.Nonce, 0, CommHeadNonceSize);
            from += CommHeadNonceSize;

            return from;
        }
    }
}
