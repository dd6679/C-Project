using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MainTree2.ViewModels;

namespace MainTree2.Views
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
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.DataContext = new LogInVM();
            _vm = this.DataContext as LogInVM;

            _vm.addCallBackWindow(LogInSuccess);
        }

        private void LogInSuccess()
        {
            if (_vm.connect)
            {
                MainWindow window = new MainWindow();
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                base.Close();
                window.Show();
            }
        }
    }
}
