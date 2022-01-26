using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Library.Client.Net.DataStruct
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
