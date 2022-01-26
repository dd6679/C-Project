using System.Windows;

namespace ClientProgram
{
    /// <summary>
    /// LogIn.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LogIn : Window
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            // 포트 번호 오류, 텍스트박스 비어있을 때 로그인 오류
            if (string.IsNullOrEmpty(ServerTextBox.Text) || PortTextBox.Text == "0" || string.IsNullOrEmpty(UserTextBox.Text) ||
                ServerTextBox.Text != "127.0.0.1" || PortTextBox.Text != "7000")
            {
                MessageBox.Show("정보를 정확히 입력해주세요", "접속 실패", MessageBoxButton.OK, MessageBoxImage.Error);
                ServerTextBox.Text = null;
                PortTextBox.Text = "0";
                UserTextBox.Text = null;
            }
            else
            {
                MainWindow mainwindow = new MainWindow();
                mainwindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                base.Close();
                mainwindow.Show();
                
            }
        }
    }
}
