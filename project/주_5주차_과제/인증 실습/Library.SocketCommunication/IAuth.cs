namespace Library.SocketCommunication
{
    interface IAuth
    {
        AuthToken AuthToken { get; set; }

        AuthInfo AuthInfo { get; set; }

        AuthResult Process(NetworkBase network);
    }
}
