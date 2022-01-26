namespace VMS.Client2.Net
{
    public enum Commands : ushort
    {
        Initialized,
        StartAuthentication,
        ExchangeAuthToken,
        CompleteAuthentication,

        UserLoiginList = 4,
        LastLoginIp = 23,
        RecvLicence = 52,
        GetServerInfo = 1001,
        KeepAlive = 1003,

        ReqServerConfig = 3001,
        TourDeviceState = 3021,
    }
}
