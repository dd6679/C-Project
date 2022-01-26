using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Client.Net.Constance
{
    public enum Commands : ushort
    {
        Initialized,
        StartAuthentication,
        ExchangeAuthToken,
        CompleteAuthentication,

        UserLoginList = 4,
        LastLoginIp = 23,
        RecvLicence = 52,
        GetServerInfo = 1001,
        KeepAlive = 1003,

        DrvRecCmdStream = 2003,

        ReqServerConfig = 3001,
        CliMstCmdQueryDeviceServer = 3002,
        DrvRecCmdDeviceStream = 3101,
        TourDeviceState = 3021,
    }
}
