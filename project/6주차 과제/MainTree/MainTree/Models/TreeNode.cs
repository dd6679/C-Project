using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainTree.Models
{
    public class TreeNode
    {
        public string Name { get; set; }
        public TreeNodeList Members { get; set; }
        public LeafNodeList LeafMembers { get; set; }
    }

    public class TreeNodeList : ObservableCollection<TreeNode> { }
    public class LeafNodeList : ObservableCollection<LeafNode> { }
}