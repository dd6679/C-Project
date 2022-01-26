using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Client.Net.Dao
{
    public class TreeNodeDao
    {
        public string SelectTreeNode(int userSerial, int ctxSerial)
        {
            return "select * from tree_node where user_serial = " + userSerial + " and ctx_serial = " + ctxSerial + " and node_type = 0";
        }

        public string SelectNodeItem(int userSerial, int ctxSerial, int nodeSerial)
        {
            return "select * from node_item where user_serial = " + userSerial + " and ctx_serial = " + ctxSerial + " and node_serial = " + nodeSerial;
        }
    }
}
