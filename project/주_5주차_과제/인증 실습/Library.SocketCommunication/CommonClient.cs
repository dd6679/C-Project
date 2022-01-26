using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using Newtonsoft.Json;

namespace Library.SocketCommunication
{
    public class CommonClient : QueueService, IDisposable
    {
        public NetworkBase _network;
        protected AuthInfo _auth;
        protected VMS.TcpIp.Net.Packet _packet;

        public CommonClient() : base()
        {
            _network = new NetworkBase();
            _auth = new AuthInfo();
            _packet = new VMS.TcpIp.Net.Packet();
        }

        #region KeepAlive // 접속 유지 메시지 요청
        public string KeepAlive()
        {
            //송신
            byte[] buff = new byte[1024];
            var packet = new VMS.TcpIp.Net.Packet { Command = (VMS.TcpIp.Net.Commands)Commands.GetLicense };
            buff = VMS.TcpIp.Net.Packet.ToArray(packet);
            _network.Send(buff);

            //수신
            buff = new byte[1024];
            int nbytes = _network.Receive(buff);
            _packet = VMS.TcpIp.Net.Packet.FromArray(buff, 0, nbytes);

            return _packet.Message;
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            _network.Close();
            _isRunning = false;
            _thread.Join();
        }
        #endregion
    }
}
