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

namespace MyCalendar
{
    /// 작업시간 : 4시간
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        TextBlock[] tbx = new TextBlock[42];
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

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            grid.Children.RemoveRange(7, 42);
            try
            {
            int year = int.Parse(textBoxY.Text);
            int month = int.Parse(textBoxM.Text);
            DisplayDate(year, month);
            }
            catch (Exception)
            {

            }
        }

        private void DisplayDate(int year, int month)
        {
            DateTime date = new DateTime(year, month, 1);
            var firstday = (int)date.AddDays(-date.Day + 1).DayOfWeek;
            var lastday = DateTime.DaysInMonth(date.Year, date.Month);

            for (int i = 1; i <= lastday; i++)
            {
                tbx[i] = new TextBlock();
                tbx[i].Text = i.ToString();
                var j = firstday + i - 1;
                Grid.SetColumn(tbx[i], j - 7 * (j / 7));
                Grid.SetRow(tbx[i], j / 7 + 1);
                grid.Children.Add(tbx[i]);
            }
        }
    }
}
