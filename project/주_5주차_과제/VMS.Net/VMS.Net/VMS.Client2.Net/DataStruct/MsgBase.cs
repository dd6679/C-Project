using System;
using VMS.Client2.Net.Constance;

namespace VMS.Client2.Net
{
    public class MsgBase
    {
        public Commands cmd { get; set; }
        public ushort val { get; set; }
        public string msg { get; set; }
        public long tran { get; set; }
        public long time { get; set; } 
      /*  public DateTime LatestUsedTime { get; set; } */  // 여러 메시지를 지속적으로 받을 때 사용

     /*   [JsonIgnore]
        public DateTime SendTime { get; set; }*/

        public MsgBase(Commands vmsCmd)
            : this()
        {
            this.cmd = vmsCmd;
        }

        public MsgBase()
        {
            /*this.SendTime = DateTime.Now;*/
            this.time = DateTime.Now.ToFileTime();
            this.tran = TranIdGenerator.NewId;
           /* this.LatestUsedTime = this.SendTime;*/
        }

     /*   public void SetBinData(byte[] data, int from, int len)
        {
            binData = data;
            binDataFrom = from;
            binDataLen = len;
        }*/

     /*   public byte[] GetBinData() { return binData; }

        public int GetBinDataFrom() { return binDataFrom; }

        public int GetBinDataLen() { return binDataLen; }

        private byte[] binData = null;
        private int binDataFrom;
        private int binDataLen;*/

        //internal Action<CommPacket> CallbackAct { get; set; }
    }
}
