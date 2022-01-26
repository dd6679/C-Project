using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VMS.Client2.Net.Constance
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
