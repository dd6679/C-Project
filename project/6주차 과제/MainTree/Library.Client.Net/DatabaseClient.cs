using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Library.Client.Net.Constance;
using Library.Client.Net.Dao;
using Library.Client.Net.DataStruct;
using Library.Client.Net.Network;
using Newtonsoft.Json;

namespace Library.Client.Net
{
    public class DatabaseClient : CommonClient, IDisposable
    {
        public ManualResetEvent mrEvent = new ManualResetEvent(false);
        public string msg, dbName;

        public override void OnProcess(Packet packet)
        {
            var command = (DatabaseCommands)(packet.Header.Command - (ushort)CommEventEnum.SrvCommandStartUser);

            switch (command)
            {
                case DatabaseCommands.Select:
                    msg = Encoding.UTF8.GetString(packet.Data, (int)PacketHeader.CommHeadSize, (int)packet.Header.DataSize);
                    mrEvent.Set();
                    break;
                //case DatabaseCommands.Update: // Update 커맨드 처리
                //case DatabaseCommands.Execute:// Execute 커맨드 처리
            }
        }

        /// /////////////////////////////////////////////////////
        /// sql 처리

        public T[] Select<T>(string sql)
        {
            var result = JsonConvert.DeserializeObject<DataResult<T>>(Select_Internal(sql));
            return result.Results;
        }

        public string Select_Internal(string sql)
        {
            DataRequest data = new DataRequest(sql, null, DatabaseCommands.Select, DataQryTypes.Plain);
            data.db = dbName;
            string json = JsonConvert.SerializeObject(data);

            // 패킷 할당 및 데이터 복사 (32바이트 이후)
            var command = (ushort)CommEventEnum.SrvCommandStartUser + (ushort)DatabaseCommands.Select;
            var packet = Packet.MakePaket((ushort)command, 0, json, 0, 0, data.tran);
            // 패킷 헤더 복사(바이트 0~15)
            PacketHeader.Encode(ref packet.Header, packet.Data, 0);

            SetTran(packet);

            _sendQueue.Add(packet);

            mrEvent.Reset();
            mrEvent.WaitOne();

            return msg;
        }

        public void Dispose()
        {
            OnDisconnected(this);
            Logout();
        }
    }
}
