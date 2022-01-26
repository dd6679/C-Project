using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Library.Client.Net;
using Library.Client.Net.Dao;
using Library.Client.Net.DataStruct;
using Library.Mvvm.Bindables;
using MainTree2.Models;

namespace MainTree2.ViewModels
{
    public class MainWindowVM : LogInVM
    {
        private new Dispatcher _dispatcher;
        private Dictionary<int, TreeNodeList> _trees = new Dictionary<int, TreeNodeList>(); //트리장비페이지번호-트리
        private Dictionary<string, int> _devNicks = new Dictionary<string, int>(); //장비이름-장비번호
        private LeafNodeList _subDirectoryList;

        #region 생성자
        public MainWindowVM()
        {
            Root = new Dictionary<string, int>();
            DirectoryList = new TreeNodeList();
            _subDirectoryList = new LeafNodeList();
            _dispatcher = Dispatcher.CurrentDispatcher;

            client.DBLogIn += SqlProcess;
        }
        #endregion

        #region Root // 콤보박스 바인딩
        private Dictionary<string, int> _root;
        public Dictionary<string, int> Root
        {
            get { return _root; }
            set { SetProperty(ref _root, value); }
        }
        #endregion

        #region 트리 모델
        private TreeNodeList directoryList;
        public TreeNodeList DirectoryList
        {
            get { return directoryList; }
            set { SetProperty(ref directoryList, value); }
        }
        #endregion

        #region 시리얼 표시 모델
        private static string _nick;
        public string Nick
        {
            get { return _nick; }
            set { SetProperty(ref _nick, value); }
        }

        private static int _dev;
        public int Dev
        {
            get { return _dev; }
            set { SetProperty(ref _dev, value); }
        }

        private static int _dch;
        public int Dch
        {
            get { return _dch; }
            set { SetProperty(ref _dch, value); }
        }

        private static int _dchm;
        public int Dchm
        {
            get { return _dchm; }
            set { SetProperty(ref _dchm, value); }
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
            foreach (var tn in _trees[kv.Value])
            {
                if (kv.Key == tn.Name)
                {
                    tn.Name = Server;
                }
            }
            DirectoryList = _trees[kv.Value];
        }
        #endregion

        #region DisplayDataInfo // 말단노드 선택 시
        private ICommand _displayDataInfo;
        public ICommand DisplayDataInfo
        {
            get { return _displayDataInfo ?? (_displayDataInfo = new RelayCommand<TreeNodeItem>(Display)); }
        }

        private void Display(TreeNodeItem node)
        {
            if (node.GetType().Name == "LeafNodeItem")
            {
                Nick = node.Name;
                Dev = _devNicks[Nick];
                var devChannel = client.database.Select<MsgReqStream>(new SerialDao().SelectDeviceChannel(Dev));
                Dch = devChannel[0].DchCh;
                var devMedia = client.database.Select<MsgReqStream>(new SerialDao().SelectDeviceMedia(Dev, Dch));
                Dchm = devMedia[0].DchmSerial;

                client.OnRecordingAccepted(Dev, Dch, Dchm);
            }
        }
        #endregion

        #region SqlProcess
        public void SqlProcess(CommonClient sender)
        {
            var userContext = client.database.Select<UserContext>(new CtxUserDao().SelectUserContext(userSerial));
            var root = new Dictionary<string, int>();

            // 콤보박스 추가
            foreach (var user in userContext)
            {
                if (!Root.ContainsKey(user.CtxDesc))
                {
                    if (user.CtxSerial != 0)
                    {
                        root.Add(user.CtxDesc, user.CtxSerial);
                        Root = root;
                        /*                        _dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                                                {
                                                    root.Add(user.CtxDesc, user.CtxSerial);
                                                    Root = root;
                                                }));*/

                        _trees[user.CtxSerial] = new TreeNodeList();
                    }
                }
            }

            // 트리 완성
            foreach (var user in userContext)
            {
                if (user.CtxSerial != 0)
                {
                    var treeNode = client.database.Select<TreeNode>(new TreeNodeDao().SelectTreeNode(userSerial, user.CtxSerial));

                    foreach (var node in treeNode)
                    {
                        _subDirectoryList = new LeafNodeList();
                        // 루트에서부터 노드 추가
                        var DirectoriesMember = new TreeNodeItem();
                        DirectoriesMember.Name = node.NodeName;

                        var nodeItem = client.database.Select<NodeItem>(new TreeNodeDao().SelectNodeItem(userSerial, node.CtxSerial, node.NodeSerial));

                        foreach (var item in nodeItem)
                        {
                            // 서브노드 추가
                            LeafNodeItem SubDirectoriesMember = new LeafNodeItem();
                            SubDirectoriesMember.Name = item.NitemNick;
                            _subDirectoryList.Add(SubDirectoriesMember);
                            DirectoriesMember.LeafMembers = _subDirectoryList;

                            // 딕셔너리에 장비닉네임을 키로 하여 장비번호 저장
                            var nodeItemDev = client.database.Select<NodeItem>(new SerialDao().SelectNodeItemDev(userSerial, item.CtxSerial, item.NodeSerial, item.NitemNick));
                            _devNicks.Add(item.NitemNick, item.DevSerial);
                        }
                        _dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => _trees[node.CtxSerial].Add(DirectoriesMember)));
                    }
                }
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
            System.Environment.Exit(0);
        }
        #endregion
    }
}
