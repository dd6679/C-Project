using System.Windows;

namespace LoginApp
{
    /// <summary>
    /// SubWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SubWindow : Window
    {
        public SubWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, RoutedEventArgs e)
        {
            base.Close();
        }
    }
}
