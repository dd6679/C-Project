using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Library.Mvvm.Bindables;
using Library.SocketCommunication;

namespace ClientProgram.ViewModels
{
    class MainWindowVM : LogInVM
    {
        private Dispatcher _dispatcher;

        #region 생성자
        public MainWindowVM()
        {
            LogList = new ObservableCollection<string>();
            _dispatcher = Dispatcher.CurrentDispatcher;

            client.addCallBackMessage(UpdateLog);
        }
        #endregion

        #region UpdateLog
        private void UpdateLog(string _msg)
        {
            _dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                new Action(() => LogList.Add(_msg)));
        }
        #endregion

        #region Message
        private string message;
        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }
        #endregion

        #region LogList // 리스트박스에 바인딩
        private ObservableCollection<string> logList;
        public ObservableCollection<string> LogList
        {
            get { return logList; }
            set { SetProperty(ref logList, value); }
        }
        #endregion

        #region SendMessage // 전송 버튼
        private ICommand sendMessage;
        public ICommand SendMessage
        {
            get { return sendMessage ?? (sendMessage = new RelayCommand(Send)); }
        }
        #endregion

        #region Send
        private void Send()
        {
            if (Message != "")
            {
                Message = "";
            }
        }
        #endregion
    }
}
