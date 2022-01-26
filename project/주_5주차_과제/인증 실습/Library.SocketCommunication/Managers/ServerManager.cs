using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Library.SocketCommunication
{
    public class ServerManager : SocketManager
    {
        private List<SessionManager> sessionManagers;
        private SessionManager session;

        #region 생성자 // 접속 대기
        public ServerManager(int port)
        {
            ServerSocket(port);

            Console.WriteLine("Message Server가 시작되었습니다.");
            Console.WriteLine(port + " 번 포트를 대기합니다.");

            sessionManagers = new List<SessionManager>();
            
            // 접속 관리 스레드
            _thread = new Thread(new ThreadStart(RunServer));
            _thread.Start();
        }
        #endregion

        #region ManageSession // 세션 관리
        private void ManageSession()
        {
            var client = _socket.Accept();
            session = new SessionManager(client);

            session._thread = new Thread(new ParameterizedThreadStart(session.RunSession));
            session._thread.Start(this);
            sessionManagers.Add(session);
        }
        #endregion

        #region RunServer // 스레드에서 접속 수락, 세션을 리스트에 넣어줌
        public void RunServer()
        {
            while (_isRunning)
            {
                ManageSession();
            }

            Close();
        }
        #endregion

        #region SendToSessions // 모든 세션에게 메시지 전송
        public void SendToSessions(string msg)
        {
            foreach (var s in sessionManagers)
            {
                s.SendData(msg);
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
