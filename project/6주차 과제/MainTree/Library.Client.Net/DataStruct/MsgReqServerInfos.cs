using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Client.Net.Constance;
using Newtonsoft.Json;

namespace Library.Client.Net.DataStruct
{
    class MsgReqServerInfos : MsgBase
    {
        public MsgReqServerInfos(int vmsId)
        {
            this.cmd = Commands.ReqServerConfig;
            this.val = 0;
            this.VmsId = vmsId;
        }

        [JsonProperty("vms_id")]
        public int VmsId { get; set; }

        [JsonProperty("srv_serial")]
        public int SrvSerial { get; set; }
    }
}
