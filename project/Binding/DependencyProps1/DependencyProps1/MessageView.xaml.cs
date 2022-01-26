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

namespace DependencyProps1
{
    /// <summary>
    /// MessageView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MessageView : UserControl
    {
        public MessageView()
        {
            InitializeComponent();
        }

        public string Message
        {
            get
            {
                return (string)GetValue(MessageProperty);
            }
            set
            {
                SetValue(MessageProperty, value);
            }
        }

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
            "Message",
            typeof(string),
            typeof(MessageView),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnMyPropertyChanged))
            );

        /// 특정 값이 변경되면 실행되는 이벤트 핸들러
        public static void OnMyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MessageView win = d as MessageView;
            // 처음에만 발생 하므로 일단 작업생략.
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var orgText = msgText.Text;
            MessageBox.Show($"[{Message}]  {orgText}");
        }
    }
}
