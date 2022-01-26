using System.Windows;
using System.Windows.Threading;
using ClientPacketParsing.ViewModels;
using Library.SocketCommunication;

namespace ClientProgram
{
    /// <summary>
    /// LogIn.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LogIn : Window
    {
        private LogInVM _vm;

        public LogIn()
        {
            InitializeComponent();
            this.DataContext = new LogInVM();
            _vm = this.DataContext as LogInVM;

            _vm.addCallBackWindow(LogInSuccess);
        }

        private void LogInSuccess()
        {
            if (_vm.connect)
            {
                MainWindow mainwindow = new MainWindow();
                mainwindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                base.Close();
                mainwindow.Show();
            }
        }
    }
}
