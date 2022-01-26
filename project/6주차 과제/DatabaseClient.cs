using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VMS.Client2.Net
{
    public delegate void DatabaseHandler(bool isSuccess, string result);
    public delegate void DataPacketHandler(Packet packet);

    public class DatabaseClient : CommonClient, IDisposable
    {
        private Dictionary<long, DataRequest> dataRequests = new Dictionary<long, DataRequest>();
        public DatabaseClient()
            : base("DatabaseClient")

        {
            _commandProcess.Regist((Commands)DatabaseCommands.Select, OnSelectReceived);
        }

        public override void OnProcess(Packet packet)
        {
            var handler = _commandProcess.Resolve((Commands)(packet.Header.Command - (ushort)CommEvents.SrvCommandStartUser));
            if (handler != null)
            {
                handler(packet);
            }
            else
            {
                var msg = Encoding.UTF8.GetString(packet.Data, (int)PacketHeader.CommHeadSize, (int)packet.Header.DataSize);
                Console.WriteLine($"Unknown Command:{(Commands)packet.Header.Command}, Data:{msg}");
            }

        }

        private void OnSelectReceived(Packet packet)
        {
            long tran = packet.Header.dstTran;
            if (dataRequests.ContainsKey(tran))
            {
                var reqObj = dataRequests[tran];
                dataRequests.Remove(reqObj.tran);

                OnSelectProcess(packet, reqObj);
            }
            else
            {
                Console.WriteLine($"OnSelectReceived, No TranId : {tran}");
            }
        }

        private void OnSelectProcess(Packet packet, DataRequest reqData)
        {
            // 대기 객체버전인가? (동기 버전)
            if (reqData.Tag is DataResSync)
            {
                var resObject = reqData.Tag as DataResSync;
                resObject.Packet = packet;
                resObject.SyncObject.Set();
            }

            // 콜백 버전인가? (비동기 버전)
            if (reqData.Tag is DataResAsync)
            {
                //Task.Run(() => {
                    var resObject = reqData.Tag as DataResAsync;
                    var recvMsg = Encoding.UTF8.GetString(packet.Data, (int)PacketHeader.CommHeadSize, (int)packet.Header.DataSize);
                    var rslt = JObject.Parse(recvMsg);
                    resObject?.DataRecieved(rslt["Message"].ToString() == "Success", rslt["Results"].ToString());
                //});
            }

            //태스크 콜백 버전인가?(비동기 버전)
            if (reqData.Tag is DataPacketAsync)
            {
                var resObject = reqData.Tag as DataPacketAsync;
                resObject?.DataRecieved(packet);
            }
        }

        public string DatabaseName { get; set; }

        #region Select<T>(sql, param, type, timeoutMilliesc) //동기식
        public DataResult<T> Select<T>(string sql, object param, DataQryTypes type, int timeoutMiliSec = 300000)
        {
            string reqString = Select_Internal(sql, param, type, DatabaseName, timeoutMiliSec);
            DataResult<T> rslt = JsonConvert.DeserializeObject<DataResult<T>>(reqString);

            return rslt;
        }

        private string Select_Internal(string sql, object param, DataQryTypes type, string databaseName, int timeoutMiliSec)
        {
            string result = string.Empty;

            var dataReq = new DataRequest(sql, param, DatabaseCommands.Select, type);
            dataReq.db = databaseName;

            SendMessage(dataReq);
           

            //////////////////////////////////////////////////////////////////
            // 동기식 수신 객체 추가
            var resObject = new DataResSync();
            dataReq.Tag = resObject;
            dataRequests.Add(dataReq.tran, dataReq);
            // 수신대기
            resObject.SyncObject.WaitOne(timeoutMiliSec);
            // 패킷처리
            var resPacket = resObject.Packet;
            return Encoding.UTF8.GetString(resPacket.Data, (int)PacketHeader.CommHeadSize, (int)resPacket.Header.DataSize);
        }

        #endregion

        #region Select(sql, param, type, callback) // 비동기식 콜백 수신
        public void Select(string sql, object param, DataQryTypes type, DatabaseHandler callback)
        {
            Select_Internal(sql, param, type, DatabaseName, callback);
        }

        private string Select_Internal(string sql, object param, DataQryTypes type, string databaseName, DatabaseHandler callback)
        {
            string result = string.Empty;
            DataRequest dataReq;

            dataReq = new DataRequest(sql, param, DatabaseCommands.Select, type);
            dataReq.db = databaseName;
            dataReq.Tag = new DataResAsync(callback);
            
            dataRequests.Add(dataReq.tran, dataReq);
            SendMessage(dataReq);

            return result;
        }


        public async Task<DataResult<T>> SelectAsync1<T>(string sql, object param, DataQryTypes type)
        {
            var source = new TaskCompletionSource<bool>();
            string result = string.Empty;   
            Select_Internal(sql, param, type, DatabaseName, (isSuccess, resString) => 
            {
                result = resString;
                source.TrySetResult(isSuccess);
            });

            await source.Task;
            return JsonConvert.DeserializeObject<DataResult<T>>(result);
        }

        #endregion

        #region SelectAsync<T>(string sql, object param, DataQryTypes type) // 비동기
        public async Task<DataResult<T>> SelectAsync<T>(string sql, object param, DataQryTypes type)
        {
            var result= await Task.Run(() => SelectAsync_Internal(sql, param, type, DatabaseName));
            return JsonConvert.DeserializeObject<DataResult<T>>(result);
        }

        private string SelectAsync_Internal(string sql, object param, DataQryTypes type, string databaseName)
        {
            Packet resPacket = default(Packet);
            var waitObject = new ManualResetEvent(false);

            var dataReq = new DataRequest(sql, param, DatabaseCommands.Select, type);
            dataReq.db = databaseName;
            dataReq.Tag = new DataPacketAsync((packet) => 
            { 
                resPacket = packet;
                waitObject.Set();
            });

            dataRequests.Add(dataReq.tran, dataReq);
            SendMessage(dataReq);

            waitObject.WaitOne();

            return Encoding.UTF8.GetString(resPacket.Data, (int)PacketHeader.CommHeadSize, (int)resPacket.Header.DataSize);
        }
        #endregion
   


        public void Dispose()
        {

        }
    }
}
