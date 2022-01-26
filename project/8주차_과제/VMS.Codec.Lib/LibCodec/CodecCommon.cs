using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMS.Codec.Lib
{
    public abstract class CodecCommon
    {
        abstract public bool DecodeVideo(byte[] buffer, int from, int len, bool bEndOfFrame = false);
        abstract public uint GetCode();
        abstract public bool SetCode(uint codecCode, VideoStreamInfo streamInfo = null); 
        abstract public bool IsValid { get; }
        abstract public bool IsReady { get; }
        abstract public IntPtr[] VideoFrame { get; }
        abstract public int[] VideoPitch { get; }
        abstract public int VideoWidth { get; }
        abstract public int VideoHeight { get; }
        abstract public void Reset();
        abstract public void Uninit();
        public bool IsNeedReset;
    }
}
