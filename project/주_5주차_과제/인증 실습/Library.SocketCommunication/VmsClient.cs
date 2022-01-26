using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace Library.SocketCommunication
{
    public class VmsClient : CommonClient, IDisposable
    {
        public delegate void receiveMessage(string _data);
        receiveMessage callbackMsg;

        public VmsClient() : base()
        {

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

        public override void Run()
        {
            manualEvent.WaitOne();
            Thread.Sleep(1000);
            callbackMessage(Queuing(_packet.Message));

            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(1000);
                callbackMessage(Queuing(KeepAlive()));
            }
        }

        public bool Login(string addr, int port, string user, string password)
        {
            try
            {
                // 접속 수락 정보 수신
                _network.Connect(addr, port, user, password);

                _auth.User = user;
                _auth.Password = password;

                byte[] buff = new byte[8192];
                int nbytes = _network.Receive(buff);
                _packet = VMS.TcpIp.Net.Packet.FromArray(buff, 0, nbytes);

                ServerInfo serverInfo = JsonConvert.DeserializeObject<ServerInfo>(_packet.Message);

                // 인증정보 전송
                var hashPass = Encoding.UTF8.GetBytes(_auth.User + ":" + _auth.Password);
                var userHash = SHA256.Create().ComputeHash(hashPass);

                var hashInfo = Encoding.UTF8.GetBytes(_auth.User + ":" + serverInfo.Server + "/" + serverInfo.Version + ":" + HexString(_packet.Nonce) + ":" + HexString(userHash));
                var authHash = SHA256.Create().ComputeHash(hashInfo);

                string str = JsonConvert.SerializeObject((object)new AuthToken()
                {
                    Token = HexString(authHash),
                    User = _auth.User,
                    Password = _auth.Password
                });

                _network.Send(VMS.TcpIp.Net.Packet.ToArray(new VMS.TcpIp.Net.Packet()
                {
                    Command = (VMS.TcpIp.Net.Commands)Commands.ExchangeAuthToken,
                    Message = str,
                    MessageLength = str.Length
                }));

                // 인증결과 수신
                buff = new byte[8192];
                nbytes = _network.Receive(buff);
                _packet = VMS.TcpIp.Net.Packet.FromArray(buff, 0, nbytes);

                AuthResult _result = JsonConvert.DeserializeObject<AuthResult>(_packet.Message);

                if (_result.result)
                {
                    manualEvent.Set();
                }

                // 결과 확인
                return _result.result;
            }

            catch (Exception e)
            {
                Console.WriteLine("에러");
                return false;
            }
        }

        private string HexString(byte[] data)
        {
            string s = BitConverter.ToString(data).Replace("-", "");
            return s.ToLower();
        }

        public void Logout()
        {
            _network.Disconnect();
        }

        #region GetServerInfor // 서버 정보 요청
        public List<Server> GetServerInfo()
        {
            // 송신
            byte[] buff = new byte[1024];
            var packet = new VMS.TcpIp.Net.Packet { Command = (VMS.TcpIp.Net.Commands)Commands.GetServerInfo };
            buff = VMS.TcpIp.Net.Packet.ToArray(packet);
            _network.Send(buff);


            // 수신
            buff = new byte[1024];
            int nbytes = _network.Receive(buff);
            _packet = VMS.TcpIp.Net.Packet.FromArray(buff, 0, nbytes);
            List<Server> servers = JsonConvert.DeserializeObject<List<Server>>(_packet.Message);

            return servers;
        }
        #endregion

        #region GetLicense // 라이센스 정보 요청
        public License GetLicense()
        {
            // 송신
            byte[] buff = new byte[1024];
            var packet = new VMS.TcpIp.Net.Packet { Command = (VMS.TcpIp.Net.Commands)Commands.GetLicense };
            buff = VMS.TcpIp.Net.Packet.ToArray(packet);
            _network.Send(buff);

            // 수신
            buff = new byte[1024];
            int nbytes = _network.Receive(buff);
            _packet = VMS.TcpIp.Net.Packet.FromArray(buff, 0, nbytes);
            License license = JsonConvert.DeserializeObject<License>(_packet.Message);

            return license;
        }
        #endregion

    }
}
