using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Library.SocketCommunication
{
    public class SocketBase
    {
        public Socket _socket;

        public SocketBase()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public bool Connect(string addr, int port)
        {
            var result = false;
            try
            {
                _socket.Connect(new IPEndPoint(IPAddress.Parse(addr), port));
                result = true;
            }
            catch(Exception)
            {

            }
            return result;
        }

        public bool Bind(int port)
        {
            var result = false;
            try
            {
                _socket.Bind(new IPEndPoint(IPAddress.Any, port));
                result = true;
            }
            catch (Exception)
            {

            }
            return result;
        }

        public void Listen(int count)
        {
            _socket.Listen(count);
        }

        public Socket Accept()
        {
            return _socket.Accept();
        }

        public void Close()
        {
            if (_socket != null)
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();

                _socket = null;
            }
        }

        public int Send(byte[] buff)
        {
            int result = 0;
            try
            {
                result = _socket.Send(buff);
            }
            catch (Exception)
            {
                result = 0;
            }
            return result;
        }

        public int Send(byte[] buff, int offset, int length)
        {
            int result = 0;
            try
            {
                result = _socket.Send(buff, length, SocketFlags.None);
            }
            catch (Exception)
            {
                result = 0;
            }
            return result;
        }

        public int Receive(byte[] buff)
        {
            int result = 0;
            try
            {
                result = _socket.Receive(buff);
            }
            catch (Exception)
            {
                result = 0;
            }
            return result;
        }

        public int Receive(byte[] buff, int offset, int length)
        {
            int result = 0;
            try
            {
                result = _socket.Receive(buff, offset, length, SocketFlags.None);
            }
            catch (Exception)
            {
                result = 0;
            }
            return result;
        }
    }
}
