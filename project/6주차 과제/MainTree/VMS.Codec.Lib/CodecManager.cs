using System;
using System.Collections.Generic;

namespace VMS.Codec.Lib
{
    // 
    public interface CodecResult
    {
        void ImageReceived(int width, int height, int bytesPerPixel, IntPtr imgBuffer, DateTime imageTime);
    }

    public class CodecManager : IDisposable
    {
        private Dictionary<string, CodecItem> Codecs { get; } = new Dictionary<string, CodecItem>();
        private object _locker = new object();    

        public void Dispose()
        {
            foreach (var item in Codecs.Values)
            {
                item.Dispose();
            }

            lock (_locker)
            {
                Codecs.Clear();
            }
        }

        public void AddCodec(string key)
        {
            lock (_locker)
            {
                if (!Codecs.ContainsKey(key))
                {
                    Codecs.Add(key, new CodecItem());
                }
            }
        }

        public CodecItem GetCodec(string key)
        {
            lock (_locker)
            {
                CodecItem item = null;

                if (Codecs.ContainsKey(key))
                {
                    item = Codecs[key];
                }

                return item;
            }
        }

        public void RemoveCodec(string key)
        {
            lock (_locker)
            {
                if (Codecs.ContainsKey(key))
                {
                    var item = Codecs[key];
                    item.Dispose();

                    Codecs.Remove(key); 
                }
            }
        }

        public void AttatchListener(string key, CodecResult result)
        {
            var item = GetCodec(key);
            item.AttachListener(result);
        }

        public void DetachListener(string key, CodecResult result)
        {
            var item = GetCodec(key);
            item.DetachListener(result);
        }


        public void AddPacket(string key, byte[] data, int from, int length)
        {
            var item = GetCodec(key);
            item.AddPacket(data, from, length);
        }


        public static string MakeKey(int vmsId, int devSerial, int dchCh, int dchmSerial)
        {
            return $"{vmsId}:{devSerial}:{dchCh}:{dchmSerial}";
        }
    }
}
