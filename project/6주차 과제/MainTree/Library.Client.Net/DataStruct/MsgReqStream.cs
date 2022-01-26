using Library.Client.Net.Constance;
using Newtonsoft.Json;

namespace Library.Client.Net.DataStruct
{
    public class MsgReqStream : MsgBase
    {
        public MsgReqStream(int vmsId, int devSerial, int dchCh, int dchmSerial, int reqType)
        {
            this.cmd = Commands.CliMstCmdQueryDeviceServer;
            this.VmsId = vmsId;
            DevSerial = devSerial;
            DchCh = dchCh;
            DchmSerial = dchmSerial;
            this.ReqType = reqType;
        }

        [JsonProperty("vms_id")]
        public int VmsId;

        [JsonProperty("dev_serial")]
        public int DevSerial;

        [JsonProperty("dch_ch")]
        public int DchCh;

        [JsonProperty("dchm_serial")]
        public int DchmSerial;

        [JsonProperty("req_type")]
        public int ReqType;
    }
}
