using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Library.Client.Net.DataStruct
{
    public class VmsServers
    {
        [JsonProperty("srv_svc_addr")]
        public string SrvAddr { get; set; }

        [JsonProperty("srv_svc_port")]
        public int SrvPort { get; set; }

        [JsonProperty("srv_serial")]
        public int SrvSerial { get; set; }
    }
}
