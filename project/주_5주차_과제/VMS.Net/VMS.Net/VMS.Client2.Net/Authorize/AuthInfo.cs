using Newtonsoft.Json;

namespace VMS.Client2.Net
{
    public enum LoginTypes { Normal, SSO }

    public class AuthInfo
    {
        public AuthInfo(string host, int port, string user, ApplicationTypes type)
        {
            Host = host;
            Port = port;
            User = user;
            Type = ApplicationTypes.AppTypeClient;
            LoginType = LoginTypes.Normal;
            SsoToken = string.Empty;
            SecurityType = 3;
        }

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("type")]
        public ApplicationTypes Type { get; set; }

        [JsonProperty("login_type")]
        public LoginTypes LoginType { get; set; }

        [JsonProperty("sso_token")]
        public string SsoToken { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
        [JsonProperty("security_type")]
        public int SecurityType { get; set; }
        [JsonProperty("cli_version")]
        public string CliVersion { get; set; }
        [JsonProperty("client_version")]
        public string ClientVersion { get; set; } = "1.7.0.0";
        [JsonProperty("reset_auth")]
        public bool ResetAuth { get; set; }
        [JsonProperty("auto_login")]
        public bool AutoLogin { get; set; } = false;
        [JsonProperty("user_serial")]
        public int UserSerial { get; set; }

        public override string ToString()
        {
            return string.Format("host: {0}, port: {1}, user: {2}", Host, Port, User);
        }
    }

    public class ClientAuthInfo : AuthInfo
    {
        public ClientAuthInfo(string host, int port, string user, ApplicationTypes type)
            : base(host, port, user, type)
        {
            Group = "group1";
        }

        [JsonProperty("grp")]
        public string Group { get; set; }
    }
}
