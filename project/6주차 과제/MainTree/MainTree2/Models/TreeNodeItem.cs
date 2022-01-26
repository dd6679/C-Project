using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainTree2.Models
{
    public class TreeNodeItem
    {
        public virtual string Name { get; set; }
        public TreeNodeList Members { get; set; }
        public LeafNodeList LeafMembers { get; set; }
    }

    public class TreeNodeList : ObservableCollection<TreeNodeItem> { }
    public class LeafNodeList : ObservableCollection<LeafNodeItem> { }
}
