using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Client.Net.DataStruct;

namespace Library.Client.Net
{
    // data는 프레임 헤더 + 데이터
    public delegate void StreamHandler(StreamInfo streamInfo, byte[] data, int from, int index);

    public class StreamManager : Dictionary<string, StreamHandler>
    {
        private static StreamManager manager = new StreamManager();

        public static StreamManager getManager()
        {
            return manager;
        }

        public void Regist(string key, StreamHandler handler)
        {
            if (!manager.ContainsKey(key))
            {
                manager.Add(key, handler);
            }
        }

        public StreamHandler Resolve(string key)
        {
            StreamHandler method = null;
            if (manager.ContainsKey(key))
            {
                method = manager[key];
            }

            return method;
        }

        public void Delete(string key)
        {
            if (manager.ContainsKey(key))
            {
                manager.Remove(key);
            }
        }
    }
}
