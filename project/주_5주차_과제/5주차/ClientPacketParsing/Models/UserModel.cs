using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientPacketParsing
{
    class UserModel
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Text { get; set; }
    }
}
