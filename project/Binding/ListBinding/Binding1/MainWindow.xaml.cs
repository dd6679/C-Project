using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Binding1
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Person person = (Person)this.FindResource("Tom"); // 직접 불러올 수 없음. 컬렉션 브로커이용. ICollectionView
            People people = (People)this.FindResource("Family");
            ICollectionView view = CollectionViewSource.GetDefaultView(people);
            Person person = (Person)view.CurrentItem;

            ++person.Age;
            MessageBox.Show($"{person.Name}의 {person.Age} 생일 축하.");
        }
    }
}
