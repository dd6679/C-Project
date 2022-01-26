using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.SocketCommunication.Constance;

namespace Library.SocketCommunication.DataStruct
{
    public class MsgBase
    {
        public Commands cmd { get; set; }
        public ushort val { get; set; }
        public string msg { get; set; }
        public long tran { get; set; }
        public long time { get; set; }

        public MsgBase(Commands vmsCmd) : this()
        {
            cmd = vmsCmd;
        }

        public MsgBase()
        {
            time = DateTime.Now.ToFileTime();
            tran = TranIdGenerator.NewId;
        }

    }
}
