using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MainTree.Models
{
    class CtxUser
    {
        [JsonProperty("user_serial")]
        public int userSerial { get; set; }

        [JsonProperty("ctx_serial")]
        public int ctxSerial { get; set; }

        [JsonProperty("ctx_desc")]
        public string ctxDesc { get; set; }

        [JsonProperty("node_serial")]
        public int nodeSerial { get; set; }

        [JsonProperty("node_name")]
        public string nodeName { get; set; }

        [JsonProperty("nitem_nick")]
        public string nitemNick { get;set; }
    }
}
