using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Client.Net.Dao
{
    public class SerialDao
    {
        public string SelectNodeItemDev(int userSerial, int ctxSerial, int nodeSerial, string nitemNick)
        {
            return "select * from node_item where user_serial = " + userSerial + " and ctx_serial = " + ctxSerial + " and node_serial = " + nodeSerial
                + " and nitem_nick = " + '"' +  nitemNick + '"';
        }

        public string SelectDeviceChannel(int devSerial)
        {
            return "select * from device_channel where dev_serial = " + devSerial;
        }

        public string SelectDeviceMedia(int devSerial, int dchCh)
        {
            return "select * from device_media where dev_serial = " + devSerial + " and dch_ch = " + dchCh;
        }
    }
}
