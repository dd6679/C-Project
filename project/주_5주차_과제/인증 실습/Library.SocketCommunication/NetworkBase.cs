using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Library.SocketCommunication
{
    public class NetworkBase
    {
        public Socket _socket;

        public NetworkBase() => _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public void Connect(string addr, int port, string user, string password)
        {
            var endpoint = new IPEndPoint(IPAddress.Parse(addr), port);
            _socket.Connect(endpoint);
        }

        public void Disconnect()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Disconnect(true);
        }

        public void Send(byte[] buff) => _socket.Send(buff);

        public int Receive(byte[] buff)
        {
            return _socket.Receive(buff);
        }

        public void Close() => _socket.Close();
    }
}
