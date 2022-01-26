using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace VMS.Codec.Lib
{
    public class CodecItem
    {
        static public class FourCC
        {
            static public uint ToInt(string val)
            {
                char[] c = val.ToCharArray();
                return ToInt(c);
            }
            static public uint ToInt(char[] c)
            {
                return (uint)((c[0] << 0) + (c[1] << 8) + (c[2] << 16) + (c[3] << 24));
            }
            static public uint ToInt(char c0, char c1, char c2, char c3)
            {
                return (uint)((c0 << 0) + (c1 << 8) + (c2 << 16) + (c3 << 24));
            }

            static public string ToString(uint val)
            {
                char[] c = { (char)((val >> 0) & 0xFF), (char)((val >> 8) & 0xFF), (char)((val >> 16) & 0xFF), (char)((val >> 24) & 0xFF) };
                return new string(c);
            }
        }

        static public CodecCommon CreateCodecFrom()
        {
            return new LibCodec();
        }

        static public CodecCommon GetCodec(uint fourCC,  VideoStreamInfo streamInfo = null) 
        {
            CodecCommon codec = null;
            bool rslt = false;
            string codecName = "";
            try
            {
                codec = CreateCodecFrom();
                if (codec != null)
                {
                    rslt = codec.SetCode(fourCC, streamInfo);
                    Debug.WriteLine(string.Format("Codec SetCode[{2}] : codecName:{0}, streamInfo:{1}", codecName, streamInfo, rslt));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            if (rslt == false)
            {
                if (codec != null)
                {
                    codec.Uninit();
                    codec = null;
                }
            }
            // try to create SW Codec
            if (codec == null ) // should prevent infinte recursive call
            {
                codec = GetCodec(fourCC, streamInfo);
            }

            return codec;
        }
    }
}
