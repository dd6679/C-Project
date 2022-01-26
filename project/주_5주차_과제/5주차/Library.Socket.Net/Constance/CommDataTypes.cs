using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SocketCommunication
{
    public enum CommDataTypes : byte
    {
        DataNone = 0x00,
        DataCommand = 0x10,
        DataStatus = 0x20,
        DataProperty = 0x30,
        DataEvent = 0x40,
        DataStreaming = 0x50,
        DataBlock = 0x60,

        SubNone = 0x00,
        SubText = 0x01,
        SubIFrame = 0x02,
        SubPFrame = 0x03,
        SubAudio = 0x04,
        SubMeta = 0x05,
        SubEvent = 0x06,
        SubBinary = 0x07,
    }
}
