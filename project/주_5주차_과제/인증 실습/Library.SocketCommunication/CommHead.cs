using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SocketCommunication
{
    public class CommHead
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
    }
}
