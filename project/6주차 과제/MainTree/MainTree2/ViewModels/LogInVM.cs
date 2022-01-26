using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Library.Client.Net;
using Library.Client.Net.DataStruct;
using Library.Mvvm.Bindables;

namespace MainTree2.ViewModels
{
    public class LogInVM : BindableBase
    {
        public static MasterClient client;
        public bool connect = false;
        public static int userSerial;
        public static int vmsId;
        public Dispatcher _dispatcher;

        public delegate void windowControl();
        windowControl window;

        #region 생성자
        public LogInVM()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
        }
        #endregion

        #region 윈도우 콜백
        public void addCallBackWindow(windowControl func)
        {
            window = func;
        }

        public void callbackWindow()
        {
            window();
        }
        #endregion

        #region Server
        protected static string _server = "172.22.41.201";
        public string Server
        {
            get { return _server; }
            set { SetProperty(ref _server, value); }
        }
        #endregion

        #region Port
        private static int _port = 7001;
        public int Port
        {
            get { return _port; }
            set { SetProperty(ref _port, value); }
        }
        #endregion

        #region User
        private static string _user = "admin";
        public string User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }
        #endregion

        #region Text
        private static string _text = "admin";
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }
        #endregion

        #region LogInBtn
        private ICommand logInBtn;
        public ICommand LogInBtn
        {
            get { return logInBtn ?? (logInBtn = new RelayCommand(LogIn)); }
        }
        #endregion

        #region LogIn
        private void LogIn()
        {
            client = new MasterClient();
            client.OnConnected += OnConnected;
            client.OnDisconnected += OnDisconnected;

            client.Login(Server, Port, User, Text);
        }
        #endregion

        #region OnConnected
        private void OnConnected(CommonClient sender)
        {
            connect = true;
            userSerial = client._authUser.UserSerial;
            vmsId = client._authUser.VmsId;
            _dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(() => callbackWindow()));
        }
        #endregion

        #region OnDisconnected
        private void OnDisconnected(CommonClient sender)
        {
            if (connect != true)
            {
                MessageBox.Show("정보를 정확히 입력해주세요", "접속 실패", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
