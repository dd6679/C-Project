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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MyClock
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // 작업 시간 : 1시간 반
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        // 타이머 1초 간격으로 업데이트
        private void Timer_Tick(object sender, EventArgs eventArgs)
        {
            Date.Text = DateTime.Now.ToString("yyyy년 MM월 dd일");
            MyClock.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }
        // 버튼 클릭 메소드
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Date.Foreground = MyClock.Foreground = Brushes.LemonChiffon;
            Background = Brushes.Coral;
            Btn.Content = "";
        }
    }
}
