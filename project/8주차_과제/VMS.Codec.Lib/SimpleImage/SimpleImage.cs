using System;
using System.Diagnostics;

namespace VMS.Codec.Lib
{
    public enum ImgResizeFormat : int
    {
        YV12 = 0,
        RGB32 = 1,
    }

    public enum ImgConvFormat : int
    {
        YV12 = 0,
        RGB32 = 1,
        RGB24 = 2,
        YUY2 = 3,
        UYVY = 4,
        YUV
    }

    public enum ImgConvModeEnum : int
    {
        Fast = 0,
        Normal
    }
    public enum ScalerTypeEnum { Simple, IScaler }
    public class SimpleImage : IDisposable
    {
        static ISimpleImage simpleImage = null;   //  interface
        static object lockObject = new object();
        private IntPtr handle = IntPtr.Zero;

        private static ISimpleImageFactory Factory
        {
            get
            {
                if (Environment.Is64BitProcess)
                    return SimpleImageFactoryWin64.Instance;
                else
                    return SimpleImageFactoryWin32.Instance;
            }
        }

        public SimpleImage()
        {
            lock (lockObject)
            {
                if (simpleImage == null)
                {
                    simpleImage = Factory.GetSimpleImage();
                    Debug.WriteLine("Create Scaler ver.1");
                }
            }
        }

        public static void ReleaseSimpleImage()
        {

        }

        int[] InPitch = new int[3];
        public void Init(
            ImgResizeFormat inFormat, int inWidth, int inHeight,
            ImgResizeFormat outFormat, int outWidth, int outHeight,
            ImgConvModeEnum resizeMode)
        {
            Close();

            InPitch[0] = 0;
            InPitch[1] = 0;
            InPitch[2] = 0;

            handle = simpleImage.Init((int)inFormat, inWidth, inHeight,
                (int)outFormat, outWidth, outHeight, (int)resizeMode);
        }

        public int Convert(IntPtr[] inBuffer, int[] inBufferPitch,
            IntPtr[] outBuffer, int[] outBufferPitch)
        {
            int ret = -1;
            try
            {
                if (inBuffer == null || outBuffer == null)
                    return ret;

                //  피치값이 없을때만 초기화 하고, 외부에서 들어오는값이 다르면 버림.
                if (InPitch[0] == 0 || InPitch[1] == 0 || InPitch[2] == 0)
                {
                    InPitch[0] = inBufferPitch[0];
                    InPitch[1] = inBufferPitch[1];
                    InPitch[2] = inBufferPitch[2];
                }

                if (InPitch[0] != inBufferPitch[0] || InPitch[1] != inBufferPitch[1] || InPitch[2] != inBufferPitch[2])
                {
                    Debug.WriteLine($"SimpleImage.Convert({InPitch[0]} != {inBufferPitch[0]} || {InPitch[1]} != {inBufferPitch[1]} || {InPitch[2]} != {inBufferPitch[2]}) Dropped!!");
                    ret = 0;
                    return ret;
                }

                ret = simpleImage.Convert(handle,
                inBuffer, inBufferPitch,
                outBuffer, outBufferPitch);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return ret;
        }


        public void SetReverse(bool bReverse)
        {
            simpleImage.SetReverse(handle, bReverse ? 1 : 0);
        }

        public void Close()
        {
            if (handle != IntPtr.Zero)
            {
                simpleImage.Close(handle);
                handle = IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            Close();
        }
    }
}
