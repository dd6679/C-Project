using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace Library.SocketCommunication
{
    public class SessionManager : SocketManager
    {
        private string userName;

        #region 생성자 // 클라이언트 접속 알림
        public SessionManager(Socket client)
        {
            _socket = client;
        }
        #endregion

        #region RunSession
        public void RunSession(object _server)
        {
            ServerManager server = _server as ServerManager;

            while (_isRunning)
            {
                try
                {
                    ReceiveData();

                    // json 데이터가 아니면 user 이름을 받은 것
                    if (!_jsonData.Contains(":"))
                    {
                        userName = _jsonData;
                        Console.WriteLine($"[{userName}] 님이 접속하였습니다.");
                        server.SendToSessions($"[{userName}] 님이 접속하였습니다.");
                    }

                    DecodingJson(_jsonData);

                    Console.WriteLine($"{_mObj.User} : {_mObj.MessageText}");

                    server.SendToSessions(_jsonData);
                }
                catch (JsonReaderException)
                {
                    continue;
                }
                catch (Exception e)
                {
                    server.RemoveSession(this);
                    Console.WriteLine($"[{userName}] 님이 접속을 종료하였습니다.");
                    Close();
                    break;
                }
            }
        }
        #endregion
    }
}