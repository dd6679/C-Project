using Newtonsoft.Json.Linq;

namespace VMS.Client2.Net
{
    internal interface IAuthProcessor
    {
        AuthHash AuthHash { get; set; }
        AuthInfo AuthInfo { get; set; }
        ServerInfo ServerInfo { get; set; }
        byte[] Nonce { get; set; }

        byte[] MakeAuthorizeToken();
        string SerializeJson();
    }

    // 인증을 위한 데이터 관리
    public class ClientAuth : IAuthProcessor
    {
        public ClientAuth(AuthInfo authInfo, string password)
        {
            this.AuthInfo = authInfo;
            this.Password = password;
        }

        public AuthHash AuthHash { get; set; }
        public AuthInfo AuthInfo { get; set; }
        public string Password { get; set; }
        private ServerInfo _serverInfo;
        public ServerInfo ServerInfo 
        {
            get { return _serverInfo; }
            set
            { 
                _serverInfo = value;
                if (_serverInfo != null)
                {
                    this.AuthHash = AuthHash.Create(_serverInfo.Version); // 버전에 따른 해시 알고리즘 선택
                    this.AuthInfo.ClientVersion = _serverInfo.Version;
                    this.AuthInfo.Version = _serverInfo.Version;
                }
            } 
        }
        public byte[] Nonce { get; set; }



        // 해시 생성
        public byte[] MakeAuthorizeToken()
        {
            return this.AuthHash.MakeHash(this.AuthInfo, this.Password, this.ServerInfo, this.Nonce);
        }

        // 해시 값을 갖는 AuthInfo를 Json 생성
        // sha256 해시길이가 32바이트 이어서 16바이트 삽입이 어려워져 Json으로 들어감.
        // 원래들어가던 16바이트 위치는 md5로 해싱하여 복사함.
        public string SerializeJson()
        {
            var tokenString = AuthHash.ToHexString(MakeAuthorizeToken());
            var authInfo = JObject.FromObject(this.AuthInfo);
            authInfo.Add("checksum", tokenString);
            return authInfo.ToString();
        }
    }
}
