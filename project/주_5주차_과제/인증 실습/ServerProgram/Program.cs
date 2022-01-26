using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Library.SocketCommunication;

namespace ServerProgram
{
    class Program
    {
        public static void UpdateLog(string msg)
        {
            Console.WriteLine(msg);
        }
        static void Main(string[] args)
        {
            var json = new JObject();
            json.Add("id", "hello");

            byte[] msg = Encoding.UTF8.GetBytes(json.ToString());
            PacketHeader head = PacketHeader.Initialize(1, 0, (uint)msg.Length, 0, 0, -1, -1);

            Packet packet = new Packet()
            {
                Head = head,
                Data = new byte[PacketHeader.CommHeadSize + PacketHeader.CommHeadNonceSize + head.DataSize],//메시지 크기에 따라 늘어나도록.
                sender = null
            };

            //Array.Copy(header, 0, packet.Data, 0, PacketHeader.CommHeadSize);
            Array.Copy(head.Nonce, 0, packet.Data, PacketHeader.CommHeadSize, PacketHeader.CommHeadNonceSize);
            Array.Copy(msg, 0, packet.Data, PacketHeader.CommHeadSize + PacketHeader.CommHeadNonceSize, msg.Length);

            // FromArray 테스트
            PacketHeader.FromArray(ref head, packet.Data, 0);

            foreach (byte b in packet.Data)
            {
                Console.Write(b);
            }

            Console.WriteLine();

            // ToArray 테스트
            PacketHeader.ToArray(packet.Data, 0, ref head);

            foreach (byte b in packet.Data)
            {
                Console.Write(b);
            }

            Console.ReadLine();
        }
    }
}
