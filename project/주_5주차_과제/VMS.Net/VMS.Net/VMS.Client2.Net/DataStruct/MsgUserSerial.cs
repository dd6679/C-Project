namespace VMS.Client2.Net
{
    internal class MsgUserSerial : MsgBase
    {
        public int vms_id { get; set; }
        public int grp_serial { get; set; }
        public int user_serial { get; set; }
    }
}
