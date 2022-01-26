using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Library.Client.Net.Authorize;
using Library.Client.Net.Constance;
using Library.Client.Net.DataStruct;
using Library.Client.Net.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Library.Client.Net
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

    public interface IPacketReceiver
    {
        bool Receive();
    }

    public delegate int ReceiveHandler(byte[] buffer, int offset, int length);
    public delegate void CompleteHandler(Packet packet);

    internal class PacketReceiver : IPacketReceiver
    {
        private Packet currentPacket;
        private enum CommSteps
        {
            RecvHeadStep,
            RecvBodyStep
        }

        private ReceiveHandler OnReceive;
        private CompleteHandler OnComplete;

        public PacketReceiver(ReceiveHandler onReceive, CompleteHandler onComplete)
        {
            OnReceive = onReceive;
            OnComplete = onComplete;
        }

        int recvBufferPos = 0;              // 수신된 데이터
        int currentPacketRecvPos = 0;       // 처리된 데이터
        int pos = 0;                        //   
        static int recvBufferSize = 8192;   // 버퍼 갯수 
        byte[] recvBuffer = new byte[recvBufferSize];

        public bool Receive()
        {
            CommSteps currentStep = CommSteps.RecvHeadStep;
            try
            {
                while (true)
                {
                    int length = OnReceive(recvBuffer, recvBufferPos, recvBufferSize - recvBufferPos);
                    if (length > 0) // 리턴 사이즈가 없거나, 모자라면 끊긴것으로 봄.
                    {
                        recvBufferPos += length;
                        while (pos < recvBufferPos)
                        {
                            if (currentStep == CommSteps.RecvHeadStep)
                            {
                                if (recvBufferPos - pos >= PacketHeader.CommHeadSize)
                                {
                                    pos = Packet.CopyHeader(recvBuffer, pos, ref currentPacket);
                                    if (pos < 0)
                                    {
                                        Console.WriteLine("Packet Header make failed"); // 헤더를 만들지 못하게 된다면 끊김.
                                        return false;
                                    }

                                    currentPacketRecvPos = PacketHeader.CommHeadSize;
                                    currentStep = CommSteps.RecvBodyStep;
                                    continue; // 헤더 처리 종료
                                }
                                else
                                {
                                    RearangeBuffer();
                                    break;
                                }
                            }
                            else
                            {
                                int len = (int)Math.Min(currentPacket.Header.AllocSize - currentPacketRecvPos, recvBufferPos - pos);
                                Buffer.BlockCopy(recvBuffer, pos, currentPacket.Data, currentPacketRecvPos, len);
                                currentPacketRecvPos += len;
                                pos += len;

                                if (currentPacketRecvPos == currentPacket.Header.AllocSize)
                                {
                                    OnComplete(currentPacket);
                                    currentStep = CommSteps.RecvHeadStep;

                                    RearangeBuffer();
                                    return true;
                                }
                                else
                                {
                                    RearangeBuffer();
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("접속 끊김");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return false;
        }

        private void RearangeBuffer()
        {
            if (pos < recvBufferPos)
            {
                Buffer.BlockCopy(recvBuffer, pos, recvBuffer, 0, recvBufferPos - pos);
                recvBufferPos = recvBufferPos - pos;
            }
            else                    // 수신데이터가 남지 않았으면 수신위치 리셋 
            {
                recvBufferPos = 0;
            }
            pos = 0;
        }
    }

    public class CommonClient : ThreadLoop//, IClientEvent
    {
        // 소켓
        private NetworkBase _network = new NetworkBase();
        // 큐
        protected QueueService _sendQueue;
        // 패킷 처리
        private IPacketReceiver _receiver;
        // 인증 프로세스
        private IAuthProcessor _authProc;
        private PacketProcessor<CommStates> _processor = new PacketProcessor<CommStates>(); // 서브 인증 프로세서

        private bool _isRunning;
        private bool IsLoggedin = false;
        public CommStates State { get; private set; }

        // 사용자 컨텍스트
        public TreeUser _authUser;
        protected static MsgReqStream _stream;

        public delegate void EventHandler(CommonClient sender);
        public EventHandler OnConnected;
        public EventHandler OnDisconnected;


        public CommonClient()
        {
            this.State = CommStates.Initialized;
            this._receiver = new PacketReceiver(_network.Receive, Process);
            this._sendQueue = new QueueService(_network);
        }

        public void Login(string addr, int port, string user, string password)
        {
            _authProc = new ClientAuth(new ClientAuthInfo(addr, port, user, ApplicationTypes.AppTypeClient), password);
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

        // 데이터베이스 접근
        public void OnDBAccepted()
        {
            MsgReqServerInfos msg = new MsgReqServerInfos(_authUser.VmsId);
            string json = JsonConvert.SerializeObject(msg);

            // 패킷 할당 및 데이터 복사 (32바이트 이후)
            var command = (ushort)CommEventEnum.SrvCommandStartUser + (ushort)msg.cmd;
            var packet = Packet.MakePaket((ushort)command, 0, json, 0, 0, msg.tran);
            // 패킷 헤더 복사(바이트 0~15)
            PacketHeader.Encode(ref packet.Header, packet.Data, 0);

            SetTran(packet);

            _sendQueue.Add(packet);
        }

        // 레코딩 접근
        public void OnRecordingAccepted(int devSerial, int dchCh, int dchmSerial)
        {
            _stream = new MsgReqStream(_authUser.VmsId, devSerial, dchCh, dchmSerial, 1);
            string json = JsonConvert.SerializeObject(_stream);

            // 패킷 할당 및 데이터 복사 (32바이트 이후)
            var command = (ushort)CommEventEnum.SrvCommandStartUser + (ushort)_stream.cmd;
            var packet = Packet.MakePaket((ushort)command, 0, json, 0, 0, _stream.tran);
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
            if (this.State != CommStates.Loggedin)
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

        public virtual void OnProcess(Packet packet) { }
    }
}

