using System;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace Library.SocketCommunication
{
    public class SessionManager : ThreadBase
    {
        public SocketBase _socket;
        private string userName;

        #region 생성자 // 클라이언트 접속 알림
        public SessionManager(Socket client)
        {
            _socket = new SocketBase();
            _socket._socket = client;
        }
        #endregion

        #region RunSession
        public override void RunObject(object _server)
        {
            ServerManager server = _server as ServerManager;
            var _mObj = new Message();

            while (_isRunning)
            {
                try
                {
                    byte[] buff = new byte[1024];
                    int nbytes = _socket.Receive(buff);

                    // 접속이 끊어졌을 때
                    if (nbytes <= 0)
                    {
                        break;
                    }

                    var jsonData = Encoding.UTF8.GetString(buff);

                    // json 데이터가 아니면 user 이름을 받은 것
                    if (!jsonData.Contains(":"))
                    {
                        userName = jsonData.Substring(0, jsonData.IndexOf('\0'));
                        Console.WriteLine($"[{userName}] 님이 접속하였습니다.");
                        server.SendToSessions($"[{userName}] 님이 접속하였습니다.");
                    }

                    _mObj = JsonConvert.DeserializeObject<Message>(jsonData);

                    Console.WriteLine($"{_mObj.User} : {_mObj.MessageText}");

                    server.SendToSessions(jsonData);
                }
                catch (JsonReaderException)
                {
                    continue;
                }
                catch (Exception)
                {
                    break;
                }
            }
            server.RemoveSession(this);
            Console.WriteLine($"[{userName}] 님이 접속을 종료하였습니다.");
            server.SendToSessions($"[{userName}] 님이 접속을 종료하였습니다.");
            _socket.Close();
            Close();
        }
        #endregion
    }
}