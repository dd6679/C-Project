using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Library.Client.Net.DataStruct
{
    public class TreeUser
    {
        [JsonProperty("user_idx")]
        public int UserIdx { get; set; }

        [JsonProperty("vms_id")]
        public int VmsId { get; set; }

        [JsonProperty("grp_serial")]
        public int GrpSerial { get; set; }

        [JsonProperty("user_serial")]
        public int UserSerial { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("user_pass")]
        public string UserPass { get; set; }

        [JsonProperty("user_property")]
        public string UserProperty { get; set; }

        [JsonProperty("user_sched")]
        public string UserSched { get; set; }

        [JsonProperty("last_layout")]
        public string LastLayout { get; set; }

        [JsonProperty("last_ptz")]
        public string LastPtz { get; set; }

        [JsonProperty("ins_time")]
        public DateTime InsTime { get; set; }

        [JsonProperty("mod_time")]
        public DateTime ModTime { get; set; }

        [JsonProperty("auth_grp_id")]
        public int AuthGrpId { get; set; }

        [JsonProperty("custom_authority")]
        public string CustomAuthority { get; set; }

        [JsonProperty("user_preferences")]
        public string UserPreferences { get; set; }

        [JsonProperty("deleted")]
        public int Delete { get; set; }

        [JsonProperty("deleted_time")]
        public DateTime Delete_time { get; set; }
    }
}
