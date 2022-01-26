using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMS.Codec.Lib
{
    public enum FrameTypeEnum : byte
    {
        IFrame = 0,
        PFrame = 1,
        BFrame = 2,
        RAPFrame = 3
    }
}
