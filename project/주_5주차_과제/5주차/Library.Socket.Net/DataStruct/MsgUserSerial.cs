using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SocketCommunication.DataStruct
{
    internal class MsgUserSerial : MsgBase
    {
        public int vms_id { get; set; }
        public int grp_serial { get; set; }
        public int user_serial { get; set; }
    }
}
