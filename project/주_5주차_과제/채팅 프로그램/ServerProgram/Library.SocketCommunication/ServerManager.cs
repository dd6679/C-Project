using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Library.SocketCommunication
{
    public class ServerManager : ThreadBase
    {
        private List<SessionManager> sessionManagers;
        private SessionManager session;
        private SocketBase _socket;

        #region 생성자 // 접속 대기
        public ServerManager(int port)
        {
            _socket = new SocketBase();
            _socket.Bind(port);
            _socket.Listen(10);

            Console.WriteLine("Message Server가 시작되었습니다.");
            Console.WriteLine(port + " 번 포트를 대기합니다.");

            sessionManagers = new List<SessionManager>();

            // 접속 관리 스레드
            Start();
        }
        #endregion

        #region ManageSession // 세션 관리
        private void ManageSession()
        {
            var client = _socket.Accept();
            session = new SessionManager(client);

            ParameterizedStart(session, this);
            sessionManagers.Add(session);
        }
        #endregion

        #region Run // 스레드에서 접속 수락, 세션을 리스트에 넣어줌
        public override void Run()
        {
            while (_isRunning)
            {
                ManageSession();
            }
            _socket.Close();
            Close();
        }
        #endregion

        #region SendToSessions // 모든 세션에게 메시지 전송
        public void SendToSessions(string msg)
        {
            foreach (var s in sessionManagers)
            {
                s._socket.Send(Encoding.UTF8.GetBytes(msg));
            }
        }
        #endregion

        #region RemoveSession // 세션 제거
        public void RemoveSession(SessionManager sender)
        {
            sessionManagers.Remove(sender);
        }
        #endregion
    }
}
