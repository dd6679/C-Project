using System;
using System.Threading;

namespace VMS.Client2.Net
{
    class Program
    {
        static void Main(string[] args)
        {
            bool running = true;
            using (var client = new MasterClinet())
            {
                client.OnConnected += (sender) => running = true;
                client.OnDisconnected += (sender) => running = false;

                client.Login("172.22.41.201", 7001, "admin", "admin");

                while(running)
                {
                    Thread.Sleep(5000);
                    client.KeepAlive();
                }


                Console.WriteLine("서버와 접속이 끊겼습니다. 아무키나 눌러주세요.");
                Console.ReadKey();
            }

        }
    }
}
