using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Library.Client.Net.DataStruct;
using MainTree2.Models;
using VMS.Codec.Lib;

namespace MainTree2.ViewModels
{
    #region Class ContentData
    public class ContentData
    {
        public int width;
        public int height;
        public int bytesPerPixel;
        public IntPtr imgBuffer;
        public DateTime imageTime;

        public ContentData(int width, int height, int bytesPerPixel, IntPtr imgBuffer, DateTime imageTime)
        {
            this.width = width;
            this.height = height;
            this.bytesPerPixel = bytesPerPixel;
            this.imgBuffer = imgBuffer;
            this.imageTime = imageTime;
        }
    }
    #endregion

    public class ContentViewVM : MainWindowVM, CodecResult
    {
        public static CodecManager _manager = new CodecManager();

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory")]
        public static extern void CopyMemory(IntPtr pDest, IntPtr pSrc, IntPtr length);

        public ContentViewVM()
        {
            SerialList = new List<SerialInfo>();
        }

        #region DevNick
        private string _devNick = "";
        public string DevNick
        {
            get { return _devNick; }
            set { SetProperty(ref _devNick, value); }
        }
        #endregion

        #region ContentData
        private ContentData _contentData;
        public ContentData ContentData
        {
            get { return _contentData; }
            set { SetProperty(ref _contentData, value); }
        }
        #endregion

        #region SerialList
        private List<SerialInfo> _serialList;
        public List<SerialInfo> SerialList
        {
            get { return _serialList; }
            set { SetProperty(ref _serialList, value); }
        }
        #endregion

        #region AddMedia // 드래그 드롭할 때
        public void AddMedia()
        {
            // 이미 스트림이 시작된 유저컨트롤이라면 현재 보여지는 스트림 멈춤
            if (DevNick != "")
            {
                string delKey = CodecManager.MakeKey(vmsId, SerialList[0].Dev, SerialList[0].Dch, SerialList[0].Dchm);
                client.StopStream(SerialList[0].Dev, SerialList[0].Dch, SerialList[0].Dchm);
                _manager.DetachListener(delKey, this);
                _manager.RemoveCodec(delKey);
                SerialList.Clear();
            }

            string addKey = CodecManager.MakeKey(vmsId, Dev, Dch, Dchm);
            _manager.AddCodec(addKey);
            _manager.AttatchListener(addKey, this);
            SerialList.Add(new SerialInfo { Nick = Nick, Dev = Dev, Dch = Dch, Dchm = Dchm });
            client.StartStream(Dev, Dch, Dchm, OnReceiveStream);
        }
        #endregion

        #region OnReceiveStream // 콜백을 받음
        public void OnReceiveStream(StreamInfo streamInfo, byte[] data, int from, int len)
        {
            try
            {
                string key = CodecManager.MakeKey(streamInfo.VmsId, streamInfo.DevSerial, streamInfo.DchCh, streamInfo.DchmSerial);
                _manager.AddPacket(key, data, from, len);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        #endregion

        #region ImageReceived // 인터페이스 구현
        public void ImageReceived(int width, int height, int bytesPerPixel, IntPtr imgBuffer, DateTime imageTime)
        {
            ContentData = new ContentData(width, height, bytesPerPixel, imgBuffer, imageTime);
        }
        #endregion
    }
}
