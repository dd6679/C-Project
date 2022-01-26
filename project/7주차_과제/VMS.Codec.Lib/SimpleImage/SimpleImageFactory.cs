using System;
using System.Runtime.InteropServices;

namespace VMS.Codec.Lib
{
    [StructLayout(LayoutKind.Sequential)]
    internal class ISimpleImage
    {
        //	Image Format
        public const int ISI_FORMAT_YUV420 = 0;
        public const int ISI_FORMAT_RGB32 = 1;
        public const int ISI_FORMAT_RGB24 = 2;
        public const int ISI_FORMAT_YUY2 = 3;
        public const int ISI_FORMAT_UYVY = 4;

        //	Convert Mode
        public const int ISI_MODE_FAST = 0; //	Fast speed
        public const int ISI_MODE_NORMAL = 1;	//	Normal Quality, Speed

        //	Deinterlace
        public const int DEINTERLACE_METHOD_SIMPLE_Y = 0;	//	Y:ODD only
        public const int DEINTERLACE_METHOD_SIMPLE_UV = 1;	//	UV 3:1
        public const int DEINTERLACE_METHOD_SMOOTH_Y = 2;	//	Y:EVEN+ODD
        public const int DEINTERLACE_METHOD_SMOOTH_UV = 3;	//	UV 1:1

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr Init_t(
            int inFormat, int inWidth, int inHeight,
            int outFormat, int outWidth, int outHeight, int mode);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int Convert_t(IntPtr handle, IntPtr[] inImage, int[] inPitch,
            IntPtr[] outImage, int[] outPitch);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void Close_t(IntPtr handle);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SetReverse_t(IntPtr handle, int bReverse);	//	Image turning over - default : false
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void YUVFormatConvert_t(IntPtr handle,
            int inFormat, IntPtr[] inImage, int[] inPitch,
            int outFormat, IntPtr[] outImage, int[] outPitch,
            int width, int height, int bReverse);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void RGBFormatConvert_t(IntPtr handle,
            int inFormat, IntPtr inImage, int inPitch,
            int outFormat, IntPtr outImage, int outPitch,
            int width, int height, int bReverse);

        //  Function Pointer
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public Init_t Init;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public Convert_t Convert;


        [MarshalAs(UnmanagedType.FunctionPtr)]
        public Close_t Close;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public SetReverse_t SetReverse;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public YUVFormatConvert_t YUVFormatConvert;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public RGBFormatConvert_t RGBFormatConvert;
    }

    internal interface ISimpleImageFactory
    {
        ISimpleImage GetSimpleImage();
        void ReleaseSimpleImage();
    }

    internal class SimpleImageFactoryWin64 : ISimpleImageFactory
    {
        static SimpleImageFactoryWin64 factory = null;

        IntPtr simpleImagePtr = IntPtr.Zero;

        static internal SimpleImageFactoryWin64 Instance
        {
            get
            {
                if (factory == null)
                    factory = new SimpleImageFactoryWin64();
                return factory;
            }
        }

        public ISimpleImage GetSimpleImage()
        {
            if (simpleImagePtr == IntPtr.Zero)
            {
                simpleImagePtr = GetISimpleImage();
            }

            return Marshal.PtrToStructure(simpleImagePtr, typeof(ISimpleImage)) as ISimpleImage;

        }

        public void ReleaseSimpleImage()
        {
            if (simpleImagePtr != IntPtr.Zero)
            {
                DeleteISimpleImage(simpleImagePtr);

                simpleImagePtr = IntPtr.Zero;
            }
        }

        [DllImport("x64/SimpleImage_x64.dll")]
        private static extern IntPtr GetISimpleImage();

        [DllImport("x64/SimpleImage_x64.dll")]
        private static extern void DeleteISimpleImage(IntPtr iOpen264D);
    }


    internal class SimpleImageFactoryWin32 : ISimpleImageFactory
    {
        static SimpleImageFactoryWin32 factory = null;

        IntPtr simpleImagePtr = IntPtr.Zero;

        static internal SimpleImageFactoryWin32 Instance
        {
            get
            {
                if (factory == null)
                    factory = new SimpleImageFactoryWin32();
                return factory;
            }
        }

        public ISimpleImage GetSimpleImage()
        {
            if (simpleImagePtr == IntPtr.Zero)
            {
                simpleImagePtr = GetISimpleImage();
            }

            return Marshal.PtrToStructure(simpleImagePtr, typeof(ISimpleImage)) as ISimpleImage;

        }

        public void ReleaseSimpleImage()
        {
            if (simpleImagePtr != IntPtr.Zero)
            {
                DeleteISimpleImage(ref simpleImagePtr);

                simpleImagePtr = IntPtr.Zero;
            }
        }

        [DllImport("Win32/SimpleImage.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetISimpleImage();

        [DllImport("Win32/SimpleImage.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DeleteISimpleImage(ref IntPtr iOpen264D);
    }
}
