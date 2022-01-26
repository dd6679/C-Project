using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MyCalendar
{
    /// 작업시간 : 4시간
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        // 작업시간 : 7시간 30분
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBoxYGotFocus(object sender, RoutedEventArgs e)
        {
            textBoxY.Clear();
        }

        private void TextBoxMGotFocus(object sender, RoutedEventArgs e)
        {
            textBoxM.Clear();
        }

    }
}
