using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Library.SocketCommunication
{
    public abstract class SocketManager
    {
        protected Socket _socket;
        protected Message _mObj = new Message();
        protected string _jsonData;

        public Thread _thread;
        public bool _isRunning = true;

        #region ServerSocket // 서버 소켓 생성
        protected void ServerSocket(int port)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, port);
            _socket.Bind(endpoint);
            _socket.Listen(10);            
        }
        #endregion

        #region ClientSocket // 클라이언트 소켓 생성
        protected void ClientSocket(string server, int port)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var endpoint = new IPEndPoint(IPAddress.Parse(server), port);
            _socket.Connect(endpoint);
        }
        #endregion

        #region Close // 종료시 호출
        protected void Close()
        {
            _isRunning = false;
            _socket.Close();
            _thread.Join();
        }
        #endregion

        #region EncodingJson // json으로 변환
        protected void EncodingJson(string user, string message)
        {
            var m = new Message { User = user, MessageText = message };
            _jsonData = JsonConvert.SerializeObject(m);
        }
        #endregion

        #region DecodingJson // json 해제
        protected void DecodingJson(string jsonData)
        {
            _mObj = JsonConvert.DeserializeObject<Message>(jsonData);
        }
        #endregion

        #region SendData // 데이터 송신
        public void SendData(string data)
        {
            byte[] buff = Encoding.UTF8.GetBytes(data);

            _socket.Send(buff);
        }
        #endregion

        #region ReceiveData // 데이터 수신
        protected void ReceiveData()
        {
            byte[] buff = new byte[1024];
            int nbytes = _socket.Receive(buff);
            _jsonData = Encoding.UTF8.GetString(buff, 0, nbytes);

            // 접속이 끊어졌을 때
            if (nbytes <= 0)
            {
                Close();
            }
        }
        #endregion
    }
}
