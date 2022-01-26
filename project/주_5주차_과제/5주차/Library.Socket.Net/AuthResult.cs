using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SocketCommunication
{
    public class AuthResult
    {
        public bool result { get; set; }
        public string msg { get; set; }
        public string user { get; set; }
        public string token { get; set; }
    }
}
