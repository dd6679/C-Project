using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMS.Client2.Net.Constance
{
    internal class TranIdGenerator
    {
        static private long _id = 0;
        static public long NewId { get { return ++_id; } }
    }
}
