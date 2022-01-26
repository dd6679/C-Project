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
            var view = GetFamilyView();
            var person = (Person)view.CurrentItem;

            ++person.Age;
            MessageBox.Show($"{person.Name}의 {person.Age} 생일 축하.");
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            var view = GetFamilyView();
            view.MoveCurrentToPrevious();
            if(view.IsCurrentBeforeFirst) // 첫 아이템을 벗어났는지 확인
            {
                view.MoveCurrentToFirst();
            }
        }

        private void foreButton_Click(object sender, RoutedEventArgs e)
        {
            var view = GetFamilyView();
            view.MoveCurrentToNext();
            if (view.IsCurrentAfterLast)// 마지막 아이템을 벗어났는지 확인
            {
                view.MoveCurrentToLast();
            }
        }

        ICollectionView GetFamilyView()
        {
            People people = (People)this.FindResource("Family");
            return CollectionViewSource.GetDefaultView(people);
        }
    }
}
