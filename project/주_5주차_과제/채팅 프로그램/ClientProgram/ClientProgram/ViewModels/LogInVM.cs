using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Library.Mvvm.Bindables;
using Library.SocketCommunication;

namespace ClientProgram.ViewModels
{
    class LogInVM : BindableBase
    {
        private static string _server = "127.0.0.1";
        public string Server
        {
            get { return _server; }
            set { SetProperty(ref _server, value); }
        }
        private static int _port = 7000;
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
    }
}
