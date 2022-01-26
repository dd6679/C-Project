using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Library.Client.Net.DataStruct
{
    public class UserContext
    {
        [JsonProperty("ctx_id")]
        public int CtxId { get; set; }

        [JsonProperty("vms_id")]
        public int VmsId { get; set; }

        [JsonProperty("grp_serial")]
        public int GrpSerial { get; set; }

        [JsonProperty("user_serial")]
        public int UserSerial { get; set; }

        [JsonProperty("ctx_serial")]
        public int CtxSerial { get; set; }

        [JsonProperty("ctx_desc")]
        public string CtxDesc { get; set; }

        [JsonProperty("ins_time")]
        public DateTime InsTime { get; set; }

        [JsonProperty("mod_time")]
        public DateTime ModTime { get; set; }
    }
}
