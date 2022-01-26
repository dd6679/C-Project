using System;
using System.Diagnostics;

namespace VMS.Codec.Lib
{
    public class LibCodec : CodecCommon, IDisposable
    {
        public static ILibCodec Factory
        {
            get
            {
                if (IntPtr.Size == 8)
                    return LibCodecFactory64.Instance;
                else
                    return LibCodecFactory32.Instance;
            }
        }

        public const int VCODEC_H264 = 1001;
        public const int VCODEC_MPEG4 = 1002;
        public const int VCODEC_H263 = 1003;
        public const int VCODEC_H263P = 1004;
        public const int VCODEC_H263I = 1005;
        public const int VCODEC_MPEG2VIDEO = 1006;
        public const int VCODEC_MJPEG = 1007;
        public const int VCODEC_H265 = 1008; // 20160218
        private uint codecCode = 0;


        IntPtr context = IntPtr.Zero;
        ILibCodec instance;

        public override string ToString()
        {
            return string.Format("LibCodec");
        }

        public LibCodec()
        {
            instance = Factory;
        }

        public override bool SetCode(uint codecCode,  VideoStreamInfo streamInfo = null)
        {
            this.codecCode = codecCode;
            switch (codecCode)
            {
                case 0x34363248://H264
                    context = Factory.InitCodec(VCODEC_H264);
                    break;
                case 0x35363248://H265
                    context = Factory.InitCodec(VCODEC_H265); // 20160218
                    break;
                case 0x3447504D://MPG4
                case 0x3456504D://MPV4
                case 0x58564944://DIVX
                case 0x44495658://XVID
                case 0x5634504D://MP4V
                    context = Factory.InitCodec(VCODEC_MPEG4);
                    break;
                case 0x47504A4D:
                case 0x4745504A:
                    context = Factory.InitCodec(VCODEC_MJPEG);
                    break;
                default:
                    //logger.Error("Not Found Code: {0}", codecCode);
                    Debug.WriteLine(string.Format("Not Found Code: {0}", codecCode));
                    break;
            }

            return context != IntPtr.Zero ? true : false;
        }

        public override uint GetCode()
        {
            return this.codecCode;
        }

        public override bool IsValid
        {
            get { return (context != IntPtr.Zero) ? true : false; }
        }

        public override bool DecodeVideo(byte[] buffer, int from, int len, bool bEndOfFrame = false)
        {
            return context != IntPtr.Zero && instance.DecodeVideo(context, buffer, from, len);
        }

        public override bool IsReady
        {
            get { return context != IntPtr.Zero && instance.GetReady(context); }
        }

        public override IntPtr[] VideoFrame
        {
            get
            {
                if (context == IntPtr.Zero)
                    return new IntPtr[] { IntPtr.Zero, IntPtr.Zero, IntPtr.Zero };
                else
                    return new IntPtr[] { instance.GetVideoFrame(context, 0), instance.GetVideoFrame(context, 1), instance.GetVideoFrame(context, 2) };
            }
        }

        public override int[] VideoPitch
        {
            get
            {
                if (context == IntPtr.Zero)
                    return new int[] { 0, 0, 0 };
                else
                    return new int[] { instance.GetVideoPitch(context, 0), instance.GetVideoPitch(context, 1), instance.GetVideoPitch(context, 2) };
            }
        }

        public override int VideoWidth
        {
            get { return context == IntPtr.Zero ? 0 : instance.GetVideoWidth(context); }
        }

        public override int VideoHeight
        {
            get { return context == IntPtr.Zero ? 0 : instance.GetVideoHeight(context); }
        }

        public override void Reset()
        {
        }

        public override void Uninit()
        {
            if (context != IntPtr.Zero)
                instance.UninitCodec(context);

            context = IntPtr.Zero;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Uninit();
        }
        #endregion
    }
}
