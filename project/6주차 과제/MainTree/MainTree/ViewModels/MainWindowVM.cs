using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Library.Mvvm.Bindables;
using MainTree.Models;
using MainTree.ViewModels;
using MySqlConnector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MainTree
{
    class MainWindowVM : LogInVM
    {
        string connectString = string.Format("Server={0};Database={1};Uid ={2};Pwd={3};", _server, "vms", "root", "pass");
        private Dictionary<int, TreeNodeList> trees = new Dictionary<int, TreeNodeList>();
        private LeafNodeList SubDirectoryList;

        #region 생성자
        public MainWindowVM()
        {
            Roots = new Dictionary<string, int>();
            DirectoryList = new TreeNodeList();
            RunSql(new CtxUserDao().SelectUserContext(userSerial));
        }
        #endregion

        #region Roots // 콤보박스 바인딩
        private Dictionary<string, int> _roots;
        public Dictionary<string, int> Roots
        {
            get { return _roots; }
            set { SetProperty(ref _roots, value); }
        }
        #endregion

        #region 디렉토리 모델
        private TreeNode directoriesMember;
        public TreeNode DirectoriesMember
        {
            get { return directoriesMember; }
            set { SetProperty(ref directoriesMember, value); }
        }

        private TreeNodeList directoryList;
        public TreeNodeList DirectoryList
        {
            get { return directoryList; }
            set { SetProperty(ref directoryList, value); }
        }
        #endregion

        #region SelectionChanged // 콤보박스 선택 변경
        private ICommand _selectionChangedCommand;
        public ICommand SelectionChangedCommand
        {
            get { return _selectionChangedCommand ?? (_selectionChangedCommand = new RelayCommand<KeyValuePair<string, int>>(OnSelectionChanged)); }
        }

        private void OnSelectionChanged(KeyValuePair<string, int> kv)
        {
            // 자기자신의 폴더명은 서버와 같게 함
            foreach (var tn in trees[kv.Value])
            {
                if (kv.Key == tn.Name)
                {
                    tn.Name = Server;
                }
            }
            DirectoryList = trees[kv.Value];
        }
        #endregion

        #region AddComboBox
        private void AddComboBox(JObject row)
        {
            var user = JsonConvert.DeserializeObject<CtxUser>(row.ToString());
            trees[user.ctxSerial] = new TreeNodeList();

            if (user.ctxSerial != 0)
            {
                // 콤보박스에 바인딩 될 값 추가
                Roots.Add(user.ctxDesc, user.ctxSerial);
                // ctx_serial의 동일 조건으로 질의
                RunSql(new TreeNodeDao().SelectTreeNode(userSerial, user.ctxSerial));
            }
        }
        #endregion

        #region AddRootNode
        private void AddRootNode(JObject row)
        {
            var node = JsonConvert.DeserializeObject<CtxUser>(row.ToString());
            SubDirectoryList = new LeafNodeList();
            // 루트에서부터 노드 추가
            DirectoriesMember = new TreeNode();
            DirectoriesMember.Name = node.nodeName;
            // node_serial의 동일 조건으로 질의
            RunSql(new TreeNodeDao().SelectNodeItem(userSerial, node.ctxSerial, node.nodeSerial));

            trees[node.ctxSerial].Add(DirectoriesMember);
        }
        #endregion

        #region AddSubNode
        private void AddSubNode(JObject row)
        {
            var node = JsonConvert.DeserializeObject<CtxUser>(row.ToString());
            // 서브노드 추가
            LeafNode SubDirectoriesMember = new LeafNode();
            SubDirectoriesMember.Name = node.nitemNick;
            SubDirectoryList.Add(SubDirectoriesMember);
            DirectoriesMember.LeafMembers = SubDirectoryList;
        }
        #endregion

        #region RunSql
        private void RunSql(string sql)
        {
            using (var conn = new MySqlConnection(connectString))
            {
                // 데이터베이스 오픈
                conn.Open();

                // 질의 실행
                var mySqlDataTable = new DataTable();
                var cmd = new MySqlCommand(sql, conn);
                var dataReader = cmd.ExecuteReader();

                // 데이터 로딩
                mySqlDataTable.Load(dataReader);

                // 데이터 Json 변환
                var colums = new List<string>();
                foreach (DataColumn col in mySqlDataTable.Columns)
                {
                    colums.Add(col.ColumnName);
                }

                var rows = new JArray();
                //SubDirectoryList = new LeafNodeList();

                foreach (DataRow page in mySqlDataTable.Rows)
                {
                    // 배열에서 객체로 변환
                    var itemArray = JObject.FromObject(page)["ItemArray"];
                    var row = new JObject();
                    var i = 0;

                    foreach (var item in itemArray)
                    {
                        row.Add(colums[i++], item);
                    }

                    // sql에 따라 메소드 호출
                    if (sql.Contains("user_context"))
                    {
                        AddComboBox(row);
                    }
                    if (sql.Contains("tree_node"))
                    {
                        AddRootNode(row);
                    }
                    if (sql.Contains("node_item"))
                    {
                        AddSubNode(row);
                    }

                    rows.Add(row);
                }
                // 데이터 객체 닫음.
                dataReader.Close();
            }
        }
        #endregion

        #region CloseWindowCommand // 윈도우 창닫기
        private ICommand _closeWindowCommand;
        public ICommand CloseWindowCommand
        {
            get { return _closeWindowCommand ?? (_closeWindowCommand = new RelayCommand(CloseWindow)); }
        }

        private void CloseWindow()
        {
            client.Dispose();
        }
        #endregion
    }
}
