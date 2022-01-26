namespace Library.SocketCommunication
{
    public enum Commands
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

        TourDeviceState = 3021,
    }
}