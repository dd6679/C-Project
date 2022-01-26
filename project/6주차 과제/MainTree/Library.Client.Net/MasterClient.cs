using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.Client.Net.Constance;
using Library.Client.Net.DataStruct;
using Library.Client.Net.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Library.Client.Net
{
    public class MasterClient : CommonClient, IDisposable
    {
        public DatabaseClient database;
        public RecordingClientManager _manager = new RecordingClientManager();

        private PacketProcessor<Commands> _processor = new PacketProcessor<Commands>(); // 서브 인증 프로세서
        private StreamManager _streamMenager = StreamManager.getManager();
        public EventHandler DBLogIn;
        private static Dictionary<int, bool> RecordingLogIn = new Dictionary<int, bool>();

        public MasterClient()
        {
            _processor.Regist(Commands.RecvLicence, RecvLicence);
            _processor.Regist(Commands.ReqServerConfig, ReqServerConfig);
            _processor.Regist(Commands.LastLoginIp, LastLoginIp);
            _processor.Regist(Commands.UserLoginList, UserLoginList);
            _processor.Regist(Commands.TourDeviceState, TourDeviceState);
            _processor.Regist(Commands.KeepAlive, KeepAlive);
            _processor.Regist(Commands.CliMstCmdQueryDeviceServer, CliMstCmdQueryDeviceServer);
        }

        public void Dispose()
        {
            OnDisconnected(this);
            Logout();
        }

        public override void OnProcess(Packet packet)
        {
            var handler = _processor.Resolve((Commands)(packet.Header.Command - (ushort)CommEventEnum.SrvCommandStartUser));
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

        /// /////////////////////////////////////////////////////
        /// 메시지 처리

        private void TourDeviceState(Packet packet)
        {
            string msg = Encoding.UTF8.GetString(packet.Data, (int)PacketHeader.CommHeadSize, (int)packet.Header.DataSize);
            Console.WriteLine($"OnCommandProcess: {msg}");
        }

        private void UserLoginList(Packet packet)
        {
            string msg = Encoding.UTF8.GetString(packet.Data, (int)PacketHeader.CommHeadSize, (int)packet.Header.DataSize);
            Console.WriteLine($"UserLoiginList: {msg}");
        }

        private void LastLoginIp(Packet packet)
        {
            string msg = Encoding.UTF8.GetString(packet.Data, (int)PacketHeader.CommHeadSize, (int)packet.Header.DataSize);
            Console.WriteLine($"LastLoginIp: {msg}");

            OnDBAccepted();
        }

        private void RecvLicence(Packet packet)
        {
            string msg = Encoding.UTF8.GetString(packet.Data, (int)PacketHeader.CommHeadSize, (int)packet.Header.DataSize);
            Console.WriteLine($"RecvLicence: {msg}");
        }

        private void KeepAlive(Packet packet)
        {
            string msg = Encoding.UTF8.GetString(packet.Data, (int)PacketHeader.CommHeadSize, (int)packet.Header.DataSize);
            Console.WriteLine($"KeepAlive: {msg}");
        }

        // 데이터베이스 로그인
        private void ReqServerConfig(Packet packet)
        {
            database = new DatabaseClient();

            string msg = Encoding.UTF8.GetString(packet.Data, (int)PacketHeader.CommHeadSize, (int)packet.Header.DataSize);
            var obj = JObject.Parse(msg);
            var inObj = obj["res"];
            var dataEngine = inObj["dataengine"];

            // 전송 시 패킷에 넣어야 하므로 저장
            var dataRequest = JsonConvert.DeserializeObject<DataRequest>(dataEngine.ToString());
            database.dbName = dataRequest.db;

            database.Login(dataEngine["host"].ToString(), int.Parse(dataEngine["port"].ToString()), "user", "user");

            while (true)
            {
                if (database.State >= CommStates.Authorized)
                {
                    DBLogIn?.Invoke(this);
                    break;
                }
            }
        }

        // 레코딩 로그인
        private void CliMstCmdQueryDeviceServer(Packet packet)
        {
            string msg = Encoding.UTF8.GetString(packet.Data, (int)PacketHeader.CommHeadSize, (int)packet.Header.DataSize);
            var obj = JObject.Parse(msg);
            var inObj = obj["res"];

            var vmsServers = JsonConvert.DeserializeObject<VmsServers>(inObj.ToString());

            RecordingClient client = new RecordingClient(_authUser.VmsId, vmsServers.SrvSerial, _stream.DevSerial);

            client.OnConnected += _manager.Add;
            client.OnDisconnected += _manager.Remove;

            if (vmsServers.SrvAddr.Contains(':'))
            {
                string[] addr = vmsServers.SrvAddr.Split(':');
                vmsServers.SrvAddr = addr.LastOrDefault();
            }

            client.Login(vmsServers.SrvAddr, vmsServers.SrvPort, "admin", "admin");

            if (client.State >= CommStates.Authorized)
            {
                RecordingLogIn[vmsServers.SrvSerial] = true;
            }
        }

        // callback 등록
        private void ReqDeviceServer(int devSerial, int dchCh, int dchmSerial, ReqTypes reqType, StreamHandler handler)
        {
            string key = _authUser.VmsId + ":" + devSerial + ":" + dchCh + ":" + dchmSerial;

            // 레코딩 클라이언트 매니저를 돌면서 해당 client를 찾는다
            foreach (var m in _manager.recordingClients.ToList())
            {
                if (m.Key == devSerial)
                {
                    var client = (RecordingClient)m.Value;

                    // req type에 따라 add나 remove
                    if (reqType == ReqTypes.Start)
                    {
                        _streamMenager.Regist(key, handler);
                    }
                    if (reqType == ReqTypes.Stop)
                    {
                        _streamMenager.Delete(key);
                    }

                    client.RequestStream();
                }
            }
        }

        //클라이언트에서 호출함
        public void StartStream(int devSerial, int dchCh, int dchmSerial, StreamHandler handler)
        {
            ReqDeviceServer(devSerial, dchCh, dchmSerial, ReqTypes.Start, handler);
        }

        public void StopStream(int devSerial, int dchCh, int dchmSerial)
        {
            ReqDeviceServer(devSerial, dchCh, dchmSerial, ReqTypes.Stop, null);
        }
    }
}
