namespace Library.SocketCommunication
{
    public enum Commands
    {
		Initialized,
		StartAuthentication,
		ExchangeAuthToken,
		CompleteAuthentication,

		GetServerInfo,
		GetLicense,
		KeepAlive
	}
}