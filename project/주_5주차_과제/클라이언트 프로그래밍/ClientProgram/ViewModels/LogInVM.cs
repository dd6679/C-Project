using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Library.Mvvm.Bindables;
using Library.SocketCommunication;

namespace ClientProgram.ViewModels
{
    class LogInVM : BindableBase
    {
        public static CommonClient client;
        public bool connect = false;

        public delegate void windowControl();
        windowControl window;

        public void addCallBackWindow(windowControl _func)
        {
            window = _func;
        }

        public void callbackWindow()
        {
            window();
        }

        private static string _server = "127.0.0.1";
        public string Server
        {
            get { return _server; }
            set { SetProperty(ref _server, value); }
        }

        private static int _port = 17001;
        public int Port
        {
            get { return _port; }
            set { SetProperty(ref _port, value); }
        }

        private static string _user;
        public string User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        private static string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        private ICommand logInBtn;
        public ICommand LogInBtn
        {
            get { return logInBtn ?? (logInBtn = new RelayCommand(LogIn)); }
        }

        private void LogIn()
        {
            client = new CommonClient();

            // 포트 번호 오류, 텍스트박스 비어있을 때 로그인 오류
            if (!client.Connect(Server, Port, User, Text))
            {
                MessageBox.Show("정보를 정확히 입력해주세요", "접속 실패", MessageBoxButton.OK, MessageBoxImage.Error);
                User = null;
                Text = null;
                connect = false;
            }
            else
            {
                connect = true;
                callbackWindow();
            }
        }
    }
}
