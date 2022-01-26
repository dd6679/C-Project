using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Library.SocketCommunication
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

    public class AuthProcessor : IAuthProcessor
    {
        public AuthProcessor(AuthInfo authInfo, string password)
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
                    this.AuthHash = new AuthHash();
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

        public string SerializeJson()
        {
            var tokenString = AuthHash.ToHexString(MakeAuthorizeToken());
            var authInfo = JObject.FromObject(this.AuthInfo);
            authInfo.Add("checksum", tokenString);
            return authInfo.ToString();
        }
    }
}
