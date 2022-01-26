using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Client.Net.Constance
{
    public enum CommStates
    {
        Initialized,
        Accepted,
        Authorized,
        Loggedin,
        DBLogin = CommEventEnum.SrvCommandStartUser + Commands.ReqServerConfig,
    }
}
