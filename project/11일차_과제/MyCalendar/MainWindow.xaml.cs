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
        Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
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
