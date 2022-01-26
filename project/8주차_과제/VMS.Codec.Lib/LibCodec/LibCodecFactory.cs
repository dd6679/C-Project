using System;
using System.IO;
using System.Runtime.InteropServices;

namespace VMS.Codec.Lib
{
    public interface ILibCodec
	{
		bool Initialize(string basePath);
		IntPtr InitCodec(int codecType);
		bool DecodeVideo(IntPtr context, byte[] buffer, int from, int len);
		bool GetReady(IntPtr context);
		IntPtr GetVideoFrame(IntPtr context, int index);
		int GetVideoPitch(IntPtr context, int index);
		int GetVideoWidth(IntPtr context);
		int GetVideoHeight(IntPtr context);
		void UninitCodec(IntPtr context);
	}
	internal class LibCodecFactory64 : ILibCodec
	{
		private static LibCodecFactory64 instance = null;
		internal static LibCodecFactory64 Instance
		{
			get
			{
				if (instance == null)
					instance = new LibCodecFactory64();

				return instance;
			}
		}

		internal LibCodecFactory64()
		{
			string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
			int index = path.LastIndexOf(Path.DirectorySeparatorChar);
			if (index >= 0)
				path = path.Substring(0, index + 1);

			int ret = LibCodecInitializeLibrary(path);
			System.Diagnostics.Debug.Assert(ret > 0);
		}

		public bool Initialize(string basePath)
		{
			return LibCodecInitializeLibrary(basePath) > 0;
		}

		public IntPtr InitCodec(int codecType)
		{
			return LibCodecInitializeCodec(codecType);
		}

		public unsafe bool DecodeVideo(IntPtr context, byte[] buffer, int from, int len)
		{
			bool rslt = false;
			try
			{
				fixed (byte* p = buffer)
				{
					IntPtr ptr = (IntPtr)p;
					rslt = LibCodecDecodeVideo(context, ptr, from, len) >= 0;
				}

			}
			catch (Exception)
			{
				throw;
			}

			return rslt;
		}


		public bool GetReady(IntPtr context)
		{
			return LibCodecDecodeReady(context) != 0;
		}

		public IntPtr GetVideoFrame(IntPtr context, int index)
		{
			return LibCodecDecodeVideoFrame(context, index);
		}

		public int GetVideoPitch(IntPtr context, int index)
		{
			return LibCodecDecodeVideoPitch(context, index);
		}

		public int GetVideoWidth(IntPtr context)
		{
			return LibCodecDecodeVideoFrameWidth(context);
		}

		public int GetVideoHeight(IntPtr context)
		{
			return LibCodecDecodeVideoFrameHeight(context);
		}

		public void UninitCodec(IntPtr context)
		{
			LibCodecUnInitializeCodec(context);
		}

		[DllImport("x64/LibCodec.dll")]
		private static extern int LibCodecInitializeLibrary([param: MarshalAs(UnmanagedType.LPStr)] string basePath);
		[DllImport("x64/LibCodec.dll")]
		private static extern IntPtr LibCodecInitializeCodec(int codecType);
		[DllImport("x64/LibCodec.dll")]
		private static extern int LibCodecDecodeReady(IntPtr context);
		[DllImport("x64/LibCodec.dll")]
		private static extern int LibCodecDecodeVideo(IntPtr context, IntPtr buffer, int from, int len);
		[DllImport("x64/LibCodec.dll")]
		private static extern IntPtr LibCodecDecodeVideoFrame(IntPtr context, int index);
		[DllImport("x64/LibCodec.dll")]
		private static extern int LibCodecDecodeVideoPitch(IntPtr context, int index);
		[DllImport("x64/LibCodec.dll")]
		private static extern int LibCodecDecodeVideoFrameWidth(IntPtr context);
		[DllImport("x64/LibCodec.dll")]
		private static extern int LibCodecDecodeVideoFrameHeight(IntPtr context);
		[DllImport("x64/LibCodec.dll")]
		private static extern void LibCodecUnInitializeCodec(IntPtr context);
	}

	internal class LibCodecFactory32 : ILibCodec
	{
		private static LibCodecFactory32 instance = null;
		internal static LibCodecFactory32 Instance
		{
			get
			{
				if (instance == null)
					instance = new LibCodecFactory32();

				return instance;
			}
		}

		internal LibCodecFactory32()
		{
			string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
			int index = path.LastIndexOf(Path.DirectorySeparatorChar);
			if (index >= 0)
				path = path.Substring(0, index + 1);

			int ret = LibCodecInitializeLibrary(path);
			System.Diagnostics.Debug.Assert(ret > 0);
		}

		public bool Initialize(string basePath)
		{
			return LibCodecInitializeLibrary(basePath) > 0;
		}

		public IntPtr InitCodec(int codecType)
		{
			return LibCodecInitializeCodec(codecType);
		}

		public bool DecodeVideo(IntPtr context, byte[] buffer, int from, int len)
		{
			return LibCodecDecodeVideo(context, buffer, from, len) >= 0;
		}

		public bool GetReady(IntPtr context)
		{
			return LibCodecDecodeReady(context) != 0;
		}

		public IntPtr GetVideoFrame(IntPtr context, int index)
		{
			return LibCodecDecodeVideoFrame(context, index);
		}

		public int GetVideoPitch(IntPtr context, int index)
		{
			return LibCodecDecodeVideoPitch(context, index);
		}

		public int GetVideoWidth(IntPtr context)
		{
			return LibCodecDecodeVideoFrameWidth(context);
		}

		public int GetVideoHeight(IntPtr context)
		{
			return LibCodecDecodeVideoFrameHeight(context);
		}

		public void UninitCodec(IntPtr context)
		{
			LibCodecUnInitializeCodec(context);
		}

		[DllImport("Win32/LibCodec.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern int LibCodecInitializeLibrary([param: MarshalAs(UnmanagedType.LPStr)] string basePath);
		[DllImport("Win32/LibCodec.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr LibCodecInitializeCodec(int codecType);
		[DllImport("Win32/LibCodec.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern int LibCodecDecodeReady(IntPtr context);
		[DllImport("Win32/LibCodec.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern int LibCodecDecodeVideo(IntPtr context, [MarshalAs(UnmanagedType.LPArray)] byte[] buffer, int from, int len);
		[DllImport("Win32/LibCodec.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr LibCodecDecodeVideoFrame(IntPtr context, int index);
		[DllImport("Win32/LibCodec.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern int LibCodecDecodeVideoPitch(IntPtr context, int index);
		[DllImport("Win32/LibCodec.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern int LibCodecDecodeVideoFrameWidth(IntPtr context);
		[DllImport("Win32/LibCodec.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern int LibCodecDecodeVideoFrameHeight(IntPtr context);

		[DllImport("Win32/LibCodec.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void LibCodecUnInitializeCodec(IntPtr context);
	}
}
