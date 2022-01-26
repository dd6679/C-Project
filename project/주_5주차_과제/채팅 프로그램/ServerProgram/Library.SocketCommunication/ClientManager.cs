using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace Library.SocketCommunication
{
    public class ClientManager : ThreadBase, IDisposable
    {
        private string _userName;
        public Queue<string> logs;
        public SocketBase _socket;
        public bool connet;

        public delegate void receiveMessage(string _data);
        receiveMessage callbackMsg;

        #region 생성자 
        public ClientManager(string server, int port, string user)
        {
            _socket = new SocketBase();
            _socket.Connect(server, port);

            logs = new Queue<string>();
            _userName = user;

            // 데이터 수신 스레드
            Start();

            _socket.Send(Encoding.UTF8.GetBytes(user));
        }
        #endregion

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

        #region Run // 큐에 메시지 저장 및 콜백
        public override void Run()
        {
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

                    // json이 아니라면 userData
                    if (!jsonData.Contains(":"))
                    {
                        string userData = jsonData.Substring(0, jsonData.IndexOf('\0'));
                        logs.Enqueue(userData);
                        callbackMessage(logs.Dequeue());
                        continue;
                    }

                    _mObj = JsonConvert.DeserializeObject<Message>(jsonData);

                    // 자기자신이 보낸 메세지
                    if (_userName == _mObj.User)
                    {
                        logs.Enqueue($"  [{_mObj.User}] {_mObj.MessageText}");
                        callbackMessage(logs.Dequeue());
                        continue;
                    }

                    // 타인이 보낸 메세지
                    logs.Enqueue($"[{_mObj.User}] {_mObj.MessageText}");
                    callbackMsg(logs.Dequeue());
                }
                catch (Exception e)
                {
                    _socket.Close();
                    break;
                }
            }
            
            Close();
        }
        #endregion

        #region RunClient
        public void RunClient(string user, string message)
        {
            var m = new Message { User = user, MessageText = message };
            var jsonData = JsonConvert.SerializeObject(m);

            _socket.Send(Encoding.UTF8.GetBytes(jsonData));
        }
        #endregion

        public void Dispose()
        {
            _socket.Close();
        }
    }
}
