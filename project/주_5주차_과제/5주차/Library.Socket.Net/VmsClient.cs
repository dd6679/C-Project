/*using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Library.SocketCommunication.Constance;
using Library.SocketCommunication.Network;
using Newtonsoft.Json;

namespace Library.SocketCommunication
{
    public class VmsClient : CommonClient, IDisposable
    {
        protected PacketProcessor<Commands> _processor = new PacketProcessor<Commands>();
        public delegate void receiveMessage(string _data);
        receiveMessage callbackMsg;

        public VmsClient()
        {
            _processor.Regist(Commands.RecvLicence, RecvLicence);
            _processor.Regist(Commands.LastLoginIp, LastLoginIp);
            _processor.Regist(Commands.UserLoginList, UserLoiginList);
            _processor.Regist(Commands.TourDeviceState, TourDeviceState);
            _processor.Regist(Commands.KeepAlive, KeepAlive);
        }

        #region addCallBackMessage // 메시지 콜백 등록
        public void addCallBackMessage(receiveMessage _func)
        {
            callbackMsg = _func;
        }
        #endregion

        #region callbackMessage // 메시지 콜백 호출
        public void callbackMessage(string _data)
        {
            callbackMsg(_data);
        }
        #endregion

        *//*        #region Run
                public override void Run()
                {
                    manualEvent.WaitOne();
                    Thread.Sleep(1000);
                    callbackMessage(Queuing(_packet.Data.ToString()));
                    for (int i = 0; i < 5; i++)
                    {
                        Thread.Sleep(1000);
                        callbackMessage(Queuing(KeepAlive()));
                    }
                }
                #endregion*//*

        public void Login(string addr, int port, string user, string password)
        {
            _auth = new AuthProcessor(new ClientAuthInfo(addr, port, user, Constance.ApplicationTypes.AppTypeClient), password);
            if (_network.Connect(addr, port))
            {
                Start();
                _sendQueue.Start();

                IsLoggedin = true;
            }
            else
            {
                State = CommStates.Initialized;
                _network.Close();
            }
        }

        public void Logout()
        {
            if (IsLoggedin)
            {
                _sendQueue.Close();

                _network.Close();

                IsLoggedin = false;
            }
        }

        public override void OnProcess(Packet packet)
        {
            var handler = _processor.Resolve((Commands)(packet.Head.Command - (ushort)CommEventEnum.SrvCommandStartUser));
            if (handler != null)
            {
                handler(packet);
            }
            else
            {
                var msg = Encoding.UTF8.GetString(packet.Data, (int)PacketHeader.CommHeadSize, (int)packet.Head.DataSize);
                Console.WriteLine($"Unknown Command:{(Commands)packet.Head.Command}, Data:{msg}");
            }
        }

        *//*        #region GetServerInfor // 서버 정보 요청
                public List<Server> GetServerInfo()
                {
                    // 송신
                    byte[] buff = new byte[1024];
                    var packet = new VMS.TcpIp.Net.Packet { Command = (VMS.TcpIp.Net.Commands)Commands.GetServerInfo };
                    Packet.ToArray(buff, 0, ref _head);
                    _network.Send(buff);


                    // 수신
                    buff = new byte[1024];
                    int nbytes = _network.Receive(buff);
                    Packet.FromArray(ref _head, buff, 0);
                    List<Server> servers = JsonConvert.DeserializeObject<List<Server>>(_packet.Data.ToString());

                    return servers;
                }
                #endregion
        */
        /*        #region GetLicense // 라이센스 정보 요청
                public License GetLicense()
                {
                    // 송신
                    byte[] buff = new byte[1024];
                    var packet = new VMS.TcpIp.Net.Packet { Command = (VMS.TcpIp.Net.Commands)Commands.GetLicense };
                    PacketHeader.ToArray(buff, 0, ref _head);
                    _network.Send(buff);

                    // 수신
                    buff = new byte[1024];
                    int nbytes = _network.Receive(buff);
                    Packet.FromArray(ref _head, buff, 0);
                    License license = JsonConvert.DeserializeObject<License>(_packet.Data.ToString());

                    return license;
                }
                #endregion*//*

        private void TourDeviceState(Packet packet)
        {
            string msg = Encoding.UTF8.GetString(packet.Data, (int)PacketHeader.CommHeadSize, (int)packet.Head.DataSize);
            Console.WriteLine($"OnCommandProcess: {msg}");
        }

        private void UserLoiginList(Packet packet)
        {
            string msg = Encoding.UTF8.GetString(packet.Data, (int)PacketHeader.CommHeadSize, (int)packet.Head.DataSize);
            Console.WriteLine($"UserLoiginList: {msg}");
        }

        private void LastLoginIp(Packet packet)
        {
            string msg = Encoding.UTF8.GetString(packet.Data, (int)PacketHeader.CommHeadSize, (int)packet.Head.DataSize);
            Console.WriteLine($"LastLoginIp: {msg}");
        }

        private void RecvLicence(Packet packet)
        {
            string msg = Encoding.UTF8.GetString(packet.Data, (int)PacketHeader.CommHeadSize, (int)packet.Head.DataSize);
            Console.WriteLine($"RecvLicence: {msg}");
        }

        private void KeepAlive(Packet packet)
        {
            string msg = Encoding.UTF8.GetString(packet.Data, (int)PacketHeader.CommHeadSize, (int)packet.Head.DataSize);
            Console.WriteLine($"KeepAlive: {msg}");
        }

        public void Dispose()
        {
            OnDisconnected(this);
            Logout();
        }
    }
}
*/