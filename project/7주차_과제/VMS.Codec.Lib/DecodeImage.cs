using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using VMS.Codec.Lib;

namespace VMS.Client.App
{ 
    public abstract class DecodeImage : IDisposable
    {
        CodecCommon codec = null;
        VideoStreamInfo streamInfo = new VideoStreamInfo();
        SimpleImage simpleImage = new SimpleImage();
        Size imageSize;
        Size desireSize;
        IntPtr[] imageBuffer = new IntPtr[3] { IntPtr.Zero, IntPtr.Zero, IntPtr.Zero };
        int[] imagePitch = new int[3] { 0, 0, 0 };

        public DecodeImage(int width = 0, int height = 0, CodecCommon codec = null) // 외부 코덱을 쓸건지 자체 코덱을 쓸건지 결정
        {
            this.codec = codec;
            this.desireSize.Width = width;
            this.desireSize.Height = height;
        }

        public bool DoDecode(byte[] data, int from, int len)
        {
            if (Decode(data, from, len))
            {
                if (Convert((int)desireSize.Width, (int)desireSize.Height))
                {
                    Distribute((int)imageSize.Width, (int)imageSize.Height, 4, imageBuffer[0], streamInfo.GetTcl());
                    return true;
                }
            }

            return false;
        }

        private bool Decode(byte[] data, int from, int len)
        {
            bool result = false;
            try
            {
                int pos = streamInfo.DecodeMemory(data, from);
                if (codec == null || codec.GetCode() != streamInfo.bih.biCompression)
                {
                    if (codec != null)
                        codec.Uninit();

                    codec = CodecItem.GetCodec(streamInfo.bih.biCompression/*, CodecUsage.ForceSW*/);
                }

                if (codec == null)
                {
                    Debug.WriteLine(string.Format("Unsupported Codec: code={0:X}", streamInfo.bih.biCompression));
                    return false;
                }

                Debug.WriteLine(string.Format("{0}x{1} = {2}, {3:x} {4} {5} ",
                    streamInfo.bih.biWidth,
                    streamInfo.bih.biHeight,
                    streamInfo.bih.biSizeImage,
                    streamInfo.bih.biCompression,
                    streamInfo.frameInfo,
                    streamInfo.GetTcl().ToString()));

                codec.DecodeVideo(data, pos, /*(int)streamInfo.bih.biSizeImage*/len, true);
                result = codec.IsReady;
                if (!result)
                    codec.DecodeVideo(data, pos, (int)streamInfo.bih.biSizeImage, true);

                result = codec.IsReady;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return result;
        }

        private bool Convert(int width, int height)
        {
            bool result = false;
            try
            {
                if (imageSize.Width != codec.VideoWidth || imageSize.Height != codec.VideoHeight)
                {
                    if (imageBuffer[0] != IntPtr.Zero)
                        Marshal.FreeHGlobal(imageBuffer[0]);

                    imageSize.Width = width == 0 ? codec.VideoWidth : width;
                    imageSize.Height = height == 0 ? codec.VideoHeight : height;
                    imagePitch[0] = (int)imageSize.Width << 2;

                    int imgSize = (int)(imagePitch[0] * imageSize.Height);
                    imageBuffer[0] = Marshal.AllocHGlobal(imgSize);

                    simpleImage.Init(ImgResizeFormat.YV12, codec.VideoWidth, codec.VideoHeight,
                                     ImgResizeFormat.RGB32, (int)imageSize.Width, (int)imageSize.Height, ImgConvModeEnum.Normal);
                }

                if (simpleImage.Convert(codec.VideoFrame, codec.VideoPitch, imageBuffer, imagePitch) <= 0)
                {
                    Debug.WriteLine("Image Conversion Failed");
                }
                result = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return result;
        }

        protected abstract void Distribute(int width, int height, int bytesPerPixel, IntPtr imgBuffer, DateTime imageTime);

        public virtual void Dispose()
        {
            if (this.codec != null)
            {
                this.codec.Uninit();
                this.codec = null;
            }

            if (imageBuffer != null && imageBuffer[0] != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(imageBuffer[0]);
                this.imageBuffer = null;
            }

            if (this.simpleImage != null)
            {
                this.simpleImage.Dispose();
                this.simpleImage = null;
            }
        }
    }
}
