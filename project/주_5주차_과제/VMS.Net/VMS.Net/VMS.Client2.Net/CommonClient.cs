using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace VMS.Client2.Net
{
    public enum CommEventEnum : ushort
    {
        EventStart = 0,
        EvtConStatChanged,
        EvtStartUser,
        EventMax = 9999,
        SrvCommandStart = 10000,
        SrvCommandAuthInfo,
        SrvCommandAuthAccept,
        SrvCommandStartUser,
        SrvCmmmandAuthDeny,
        SrvCommandMax = 19999,
        CliCommandStart = 20000,
        CliCommandLogin,
        CliCommandStartUser,
        CliCommandMax = 29999
    }

    public interface IPacketReciver
    {
        bool Receive();
    }

    class PacketReciver : IPacketReciver
    {
        private Packet currentPacket;
        enum CommSteps
        {
            RecvHeadStep,
            RecvBodyStep
        }

        public delegate int ReceiveHandler(byte[] buffer, int offset, int length);
        ReceiveHandler OnReceive;
        public delegate void CompleteHandler(Packet packet);
        CompleteHandler OnComplete;

        public PacketReciver(ReceiveHandler onReceive, CompleteHandler onComplete)
        {
            OnReceive = onReceive;
            OnComplete = onComplete;
        }

        public bool Receive()
        {
            int recvBufferPos = 0;         // 수신된 데이터
            int currentPacketRecvPos = 0;  // 처리된 데이터
            int pos = 0;                   //   
            int recvBufferSize = 8192;     // 버퍼 갯수 
            CommSteps currentStep = CommSteps.RecvHeadStep;

            try
            {
                byte[] recvBuffer = new byte[recvBufferSize];
                int length = OnReceive(recvBuffer, recvBufferPos, recvBufferSize - recvBufferPos);
                if (length > 0)
                {
                    recvBufferPos += length;
                    while (pos < recvBufferPos)
                    {
                        if (currentStep == CommSteps.RecvHeadStep)// 헤더 스텝
                        {
                            if (recvBufferPos - pos >= PacketHeader.CommHeadSize)
                            {
                                // 헤더 복사
                                pos = Packet.CopyHeader(recvBuffer, pos, ref currentPacket);
                                if (pos < 0)
                                {
                                    // 헤더를 만들지 못 할 경우.
                                    //Close();
                                    return false;
                                }

                                currentPacketRecvPos = PacketHeader.CommHeadSize;
                                currentStep = CommSteps.RecvBodyStep;
                            }
                            else
                            {
                                if (pos < recvBufferPos)
                                {
                                    Buffer.BlockCopy(recvBuffer, pos, recvBuffer, 0, recvBufferPos - pos);
                                    recvBufferPos = recvBufferPos - pos;
                                }
                                else
                                    recvBufferPos = 0;

                                break;
                            }
                        }
                        else// 바디 스텝
                        {

                            int len = (int)Math.Min(currentPacket.Header.AllocSize - currentPacketRecvPos, recvBufferPos - pos);
                            Buffer.BlockCopy(recvBuffer, pos, currentPacket.Data, currentPacketRecvPos, len);
                            currentPacketRecvPos += len;
                            pos += len;

                            if (currentPacketRecvPos == currentPacket.Header.AllocSize)
                            {
                                currentStep = CommSteps.RecvHeadStep;

                                // 패킷 프로세스
                                OnComplete(currentPacket);
                            }
                        }
                    }
                }
                else
                {
                    //Console.WriteLine("접속 끊김");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return true;
        }
    }

    public class CommonClient : ThreadLoop//, IClientEvent
    {
        // 소켓
        private NetworkBase _network = new NetworkBase();
        // 큐
        private QueueThread _sendQueue;
        // 패킷 처리
        private IPacketReciver _receiver; 
        // 인증 프로세스
        private IAuthProcessor _authProc;
        private PacketProcessor<CommStates> _processor = new PacketProcessor<CommStates>(); // 서브 인증 프로세서

        private bool _isRunning;
        private bool IsLoggedin = false;
        public CommStates State { get; private set; }

        // 사용자 컨텍스트
        private TreeUser _authUser;

        public delegate void EventHandler(CommonClient sender);
        public EventHandler OnConnected;
        public EventHandler OnDisconnected;


        public CommonClient()
        {
            this.State = CommStates.Initialized;
            this._receiver = new PacketReciver(_network.Receive, Process);
            this._sendQueue = new QueueThread(_network);
        }
        public void Login(string addr, int port, string user, string password)
        {
            _authProc = new ClientAuth(new ClientAuthInfo(addr, port, user, ApplicationTypes.AppTypeClient), password );
            if (_network.Connect(addr, port))
            {
                Console.WriteLine("Connection Accepted!");
                Start(); // 수신 스레드시작
                _sendQueue.Start();
                Console.WriteLine("Queue Ready!");

                IsLoggedin = true;
            }
            else
            {
                this.State = CommStates.Initialized;
                _network.Close();
            }
        }

        public void Logout()
        {
            if (IsLoggedin)
            {
                _sendQueue.Close();
                Console.WriteLine("Queue Closed");
                _network.Close();
                Console.WriteLine("Network Disconnected!");

                IsLoggedin = false;
            }
        }

        public void KeepAlive()
        {
            if (_authUser == null)
            {
                Console.WriteLine("Not authorized!!!");
                return;
            }

            MsgUserSerial msg = new MsgUserSerial()
            {
                cmd = Commands.KeepAlive,
                val = (ushort)0,
                vms_id = _authUser.VmsId,
                grp_serial = _authUser.GrpSerial,
                user_serial = _authUser.UserSerial,
            };
            string json = JsonConvert.SerializeObject(msg);

            // 패킷 할당 및 데이터 복사 (32바이트 이후)
            var command = (ushort)CommEventEnum.SrvCommandStartUser + (ushort)msg.cmd;
            var packet = Packet.MakePaket((ushort)command, 0, json, 0, 0, msg.tran);
            // 패킷 헤더 복사(바이트 0~15)
            PacketHeader.Encode(ref packet.Header, packet.Data, 0);

            SetTran(packet);

            _sendQueue.Add(packet);
        }

        public override void Close()
        {
            this.State = CommStates.Initialized;
            _isRunning = false;
            base.Close();
        }

        public override bool IsLoopRun
        {
            get
            {
                return _isRunning;
            }
        }

        public override bool IsThreadPause
        {
            get
            {
                return false;
            }
        }

        // 인증 프로세싱

        public override void Initialize()
        {
            _isRunning = true;

            _processor.Regist(CommStates.Initialized, OnInitialized);        // 접속 초기화
            _processor.Regist(CommStates.Accepted, OnAccepted);              // 접속수락 / 토큰 전송
            _processor.Regist(CommStates.Authorized, OnAuthorized);          // 토큰 수신
            _processor.Regist(CommStates.Loggedin, OnCommandProcess);        // 인증완료

            OnInitialized(new Packet()); // 상태만 바꿔주기위해
        }

        public override bool Run()
        {
            return this._receiver.Receive();
        }

        public override void Exit()
        {
            Console.WriteLine("Thread Loop Complete");

            OnDisconnected(this);
        }

        public virtual void Process(Packet packet)
        {
            // 패킷 처리
            var handler = _processor.Resolve(this.State);
            if (handler != null)
            {
                handler(packet);
            }
            else
            {
                Console.WriteLine($"Unknown State:{this.State}");
            }
        }

        // 인증 스텝별 프로시저

        // 초기값
        private void OnInitialized(Packet packet)
        {
            Console.WriteLine("Authorizing OnInitialized");

            this.State = CommStates.Accepted;
        }

        private void OnAccepted(Packet packet)
        {
            Console.WriteLine("Authorizing OnAccepted");

            string msg = Encoding.UTF8.GetString(packet.Data, (int)PacketHeader.CommHeadSize, (int)packet.Header.DataSize);
            var serverInfo = JsonConvert.DeserializeObject<ServerInfo>(msg);

            _authProc.ServerInfo = serverInfo;// 해싱 방법을 선택한다.
            _authProc.Nonce = packet.Header.Nonce;// nonce를 추출한다.
            
            SendAuthInfo(_authProc.SerializeJson()); // 전송

            this.State = CommStates.Authorized;
        }

        private void SendAuthInfo(string hashJson)
        {
            // 패킷 할당 및 데이터 복사 (32바이트 이후)
            var packet = Packet.MakePaket((ushort)CommEventEnum.CliCommandLogin, 0, hashJson, 0, 0);
            // 패킷 헤더 복사(바이트 0~15)
            PacketHeader.Encode(ref packet.Header, packet.Data, 0);
            if(this.State != CommStates.Loggedin)
            {
                // 체크섬 복사 (바이트 16~31)
                AddChecksum(ref packet, _authProc.MakeAuthorizeToken(), _authProc.Nonce);
            }

            _sendQueue.Add(packet);
        }

        private void OnAuthorized(Packet packet)
        {
            Console.WriteLine("Authorizing OnAuthorized");

            string msg = Encoding.UTF8.GetString(packet.Data, (int)PacketHeader.CommHeadSize, (int)packet.Header.DataSize);
            var result = JsonConvert.DeserializeObject<Result<TreeUser>>(msg);
            _authUser = result.Param;
            if (_authUser != null)
            {
                var obj = JObject.Parse(msg);
                var inObj = obj["param"];
                if (obj["checksum"] != null)
                {
                    var clienmade = AuthHash.ToHexString(_authProc.MakeAuthorizeToken());
                    var servermade = obj["checksum"].ToString();

                    if (clienmade == servermade)
                    {
                        var userSerial = inObj["user_serial"];
                        int a = _authUser.UserSerial;
                        Console.WriteLine("인증 성공");
                        OnConnected(this);
                    }
                }
            }

            this.State = CommStates.Loggedin;
        }

        private void OnCommandProcess(Packet packet)
        {
            OnProcess(packet);
        }

        public void AddChecksum(ref Packet packet, byte[] ha1array, byte[] serverNonce)
        {
            MD5 md5hash = MD5.Create();
            int len = ha1array.Length + serverNonce.Length + PacketHeader.CommHeadNonceFrom;

            byte[] buffer = new byte[len];
            Buffer.BlockCopy(ha1array, 0, buffer, 0, ha1array.Length);
            Buffer.BlockCopy(serverNonce, 0, buffer, ha1array.Length, serverNonce.Length);
            Buffer.BlockCopy(packet.Data, 0, buffer, ha1array.Length + serverNonce.Length, PacketHeader.CommHeadNonceFrom);
            packet.Header.Nonce = md5hash.ComputeHash(buffer);
            Buffer.BlockCopy(packet.Header.Nonce, 0, packet.Data, PacketHeader.CommHeadNonceFrom, PacketHeader.CommHeadNonceSize);
        }

        public static void SetTran(Packet packet)
        {
            int from = PacketHeader.CommHeadNonceFrom;
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(packet.Header.srcTran)), 0, packet.Data, from, 8);
            from += 8;
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(packet.Header.dstTran)), 0, packet.Data, from, 8);
        }

        public virtual void OnProcess(Packet packet){ }
    }


    // 마스터 클라이언트 
    public class MasterClinet : CommonClient, IDisposable
    {
        protected PacketProcessor<Commands> _processor = new PacketProcessor<Commands>(); // 서브 인증 프로세서
        public MasterClinet()
        {
            _processor.Regist(Commands.RecvLicence, RecvLicence);
            _processor.Regist(Commands.LastLoginIp, LastLoginIp);
            _processor.Regist(Commands.UserLoiginList, UserLoiginList);
            _processor.Regist(Commands.TourDeviceState, TourDeviceState);
            _processor.Regist(Commands.KeepAlive, KeepAlive);
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

        private void UserLoiginList(Packet packet)
        {
            string msg = Encoding.UTF8.GetString(packet.Data, (int)PacketHeader.CommHeadSize, (int)packet.Header.DataSize);
            Console.WriteLine($"UserLoiginList: {msg}");
        }

        private void LastLoginIp(Packet packet)
        {
            string msg = Encoding.UTF8.GetString(packet.Data, (int)PacketHeader.CommHeadSize, (int)packet.Header.DataSize);
            Console.WriteLine($"LastLoginIp: {msg}");
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

    }
}
