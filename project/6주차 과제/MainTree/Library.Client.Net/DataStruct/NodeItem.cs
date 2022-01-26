using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Library.Client.Net.DataStruct
{
    public class NodeItem
    {
        [JsonProperty("vms_id")]
        public int VmsId { get; set; }

        [JsonProperty("grp_serial")]
        public int GrpSerial { get; set; }

        [JsonProperty("user_serial")]
        public int UserSerial { get; set; }

        [JsonProperty("ctx_serial")]
        public int CtxSerial { get; set; }

        [JsonProperty("node_serial")]
        public int NodeSerial { get; set; }

        [JsonProperty("node_type")]
        public int NodeType { get; set; }

        [JsonProperty("dev_serial")]
        public int DevSerial { get; set; }

        [JsonProperty("nitem_nick")]
        public string NitemNick { get; set; }

        [JsonProperty("nitem_index")]
        public int NitemIndex { get; set; }

        [JsonProperty("ins_time")]
        public DateTime InsTime { get; set; }

        [JsonProperty("mod_time")]
        public DateTime ModTime { get; set; }
    }
}
