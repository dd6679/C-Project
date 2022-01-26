using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace VMS.Codec.Lib
{
    internal class RecvData
    {
        public RecvData(byte[] data,  int from, int length)
        {
            Data = data;
            From = from;
            Length = length;
        }

        public byte[] Data { get; set; }
        public int From { get; set; }
        public int Length { get; set; }
    }

    public class CodecItem : IDisposable
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

        private Thread _thread;
        private ManualResetEvent _mrEvent = new ManualResetEvent(false);
        private Queue<RecvData> _QueueDatas = new Queue<RecvData>();
        private object _dataLocker = new object();
        private bool isRunning = false;


        private List<CodecResult> _CodecResults = new List<CodecResult>();
        private object _liestenerLocker = new object();

        CodecCommon _codec = null;


        SimpleImage simpleImage = new SimpleImage();
        Size imageSize;
        //Size desireSize;
        IntPtr[] _imageBuffer = new IntPtr[3] { IntPtr.Zero, IntPtr.Zero, IntPtr.Zero };
        int[] imagePitch = new int[3] { 0, 0, 0 };

        static public CodecCommon CreateCodec()
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
                codec = CreateCodec();
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

        public CodecItem()
        {
            _codec = CreateCodec();

            Start();
        }
        public void Dispose()
        {
            Stop();

            lock (_dataLocker)
            {
                _QueueDatas.Clear();
            }

            lock (_liestenerLocker)
            {
                _CodecResults.Clear();
            }
        }

        public void AttachListener(CodecResult result)
        {
            lock (_liestenerLocker)
            {
                _CodecResults.Add(result);
            }
        }

        public void DetachListener(CodecResult result)
        {
            lock (_liestenerLocker)
            {
                _CodecResults.Remove(result);
            }
        }

        private void Start()
        {
            isRunning = true;
            _thread = new Thread(new ThreadStart(Run));
            _thread.Start();
        }

        private void Stop()
        {
            isRunning = false;
            _mrEvent.Set();
            _thread.Join();
        }

        private void Run()
        {
            while(isRunning)
            {
                var item = GetRecvData();
                if (item == null)
                {
                    _mrEvent.WaitOne(33);
                    continue;
                }

                if (DecodeVideo(item))
                {
                    if (Convert(_codec.VideoWidth, _codec.VideoHeight))
                    {
                        Distribute();
                    }
                }
            }
        }

        private RecvData GetRecvData()
        {
            lock (_dataLocker)
            {
                RecvData item = null;
                if (_QueueDatas.Count > 0)
                {
                    item = _QueueDatas.Dequeue();
                    if (_QueueDatas.Count == 0)
                        _mrEvent.Reset();
                }
                return item;
            }
        }

        VideoStreamInfo _streamInfo = new VideoStreamInfo();
        private bool DecodeVideo(RecvData data)
        {
            bool result = false;
            try
            {
                int pos = _streamInfo.DecodeMemory(data.Data, data.From);
                if (_codec == null || _codec.GetCode() != _streamInfo.bih.biCompression)
                {
                    if (_codec != null)
                        _codec.Uninit();

                    _codec = CodecItem.GetCodec(_streamInfo.bih.biCompression);
                }

                if (_codec == null)
                {
                    Debug.WriteLine(string.Format("Unsupported Codec: code={0:X}", _streamInfo.bih.biCompression));
                    return false;
                }

                Debug.WriteLine(string.Format("{0}x{1} = {2}, {3:x} {4} {5} ",
                    _streamInfo.bih.biWidth,
                    _streamInfo.bih.biHeight,
                    _streamInfo.bih.biSizeImage,
                    _streamInfo.bih.biCompression,
                    _streamInfo.frameInfo,
                    _streamInfo.GetTcl().ToString()));

                _codec.DecodeVideo(data.Data, pos, data.Length);
                result = _codec.IsReady;
                if (!result)
                    _codec.DecodeVideo(data.Data, pos, (int)_streamInfo.bih.biSizeImage);

                result = _codec.IsReady;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return result;
        }

        private void Distribute()
        {
            for (int i = 0; i < _CodecResults.Count; i++)
            {
                _CodecResults[i].ImageReceived(_codec.VideoWidth, _codec.VideoHeight, 4, _imageBuffer[0], _streamInfo.GetTcl());
            }
        }

        private bool Convert(int width, int height)
        {
            bool result = false;
            try
            {
                if (imageSize.Width != _codec.VideoWidth || imageSize.Height != _codec.VideoHeight)
                {
                    if (_imageBuffer[0] != IntPtr.Zero)
                        Marshal.FreeHGlobal(_imageBuffer[0]);

                    imageSize.Width = width == 0 ? _codec.VideoWidth : width;
                    imageSize.Height = height == 0 ? _codec.VideoHeight : height;
                    imagePitch[0] = (int)imageSize.Width << 2;

                    int imgSize = (int)(imagePitch[0] * imageSize.Height);
                    _imageBuffer[0] = Marshal.AllocHGlobal(imgSize);

                    simpleImage.Init(ImgResizeFormat.YV12, _codec.VideoWidth, _codec.VideoHeight,
                                     ImgResizeFormat.RGB32, (int)imageSize.Width, (int)imageSize.Height, ImgConvModeEnum.Normal);
                }

                if (simpleImage.Convert(_codec.VideoFrame, _codec.VideoPitch, _imageBuffer, imagePitch) <= 0)
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

        public void AddPacket(byte[] data, int from, int length)
        {
            lock (_dataLocker)
            {
                _QueueDatas.Enqueue(new RecvData(data, from, length));
                _mrEvent.Set();
            }
        }
    }
}
