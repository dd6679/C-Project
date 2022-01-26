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
        static void Main(string[] args)
        {
            ServerManager server = new ServerManager(7000);
            //server.Close();
            Console.ReadLine();
        }
    }
}
