using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Library.SocketCommunication
{
    public class ServerInfo
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("security_type")]
        public int Security_type { get; set; }

        [JsonProperty("server")]
        public string Server { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }
}
