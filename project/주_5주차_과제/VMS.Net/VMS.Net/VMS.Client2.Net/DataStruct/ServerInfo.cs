using Newtonsoft.Json;

namespace VMS.Client2.Net
{
    public class ServerInfo
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("security_type")]
        public int SecurityType { get; set; }

        [JsonProperty("server")]
        public string Server { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }
}
