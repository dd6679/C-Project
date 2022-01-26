using System.Windows;

namespace LoginApp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SubWindowOpen_Click(object sender, RoutedEventArgs e)
        {
            SubWindow sub = new SubWindow();
            sub.Owner = this;
            sub.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            sub.ShowDialog();
        }
        private void Window_Closing(object sender, RoutedEventArgs e)
        {
            base.Close();
        }
    }
}
