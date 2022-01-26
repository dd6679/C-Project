using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Library.SocketCommunication
{
    public class ClientManager : SocketManager
    {
        public Queue<string> logs;
        public delegate void receiveMessage(string _data);
        receiveMessage callbackMsg;

        #region 생성자 
        public ClientManager(string server, int port, string user)
        {

            ClientSocket(server, port);

            logs = new Queue<string>();

            // 데이터 수신 스레드
            _thread = new Thread(new ThreadStart(EnqueueLog));
            _thread.Start();

            SendData(user);
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

        #region EnqueueLog // 큐에 메시지 저장 및 콜백
        private void EnqueueLog()
        {
            while (_isRunning)
            {
                try
                {
                    ReceiveData();

                    // json이 아니라면 userData
                    if (!_jsonData.Contains(":"))
                    {
                        string userData = _jsonData;
                        logs.Enqueue(userData);
                        callbackMessage(logs.Dequeue());
                        continue;
                    }

                    DecodingJson(_jsonData);

                    logs.Enqueue($"[{_mObj.User}] {_mObj.MessageText}");
                    callbackMsg(logs.Dequeue());
                }
                catch (Exception e)
                {
                    Close();
                    break;
                }
            }

            Close();
        }
        #endregion

        #region RunClient
        public void RunClient(string user, string message)
        {
            EncodingJson(user, message);

            SendData(_jsonData);
        }
        #endregion
    }
}
