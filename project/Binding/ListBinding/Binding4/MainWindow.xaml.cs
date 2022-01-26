using Library.Mvvm.Bindables;
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

            mainGrid.DataContext = new MainWindowViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var view = GetFamilyView();
            //var person = view.FirstOrDefault();

            //++person.Age;
            //MessageBox.Show($"{person.Name}의 {person.Age} 생일 축하.");
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            //var view = GetFamilyView();
            //view.MoveCurrentToPrevious();
            //if(view.IsCurrentBeforeFirst) // 첫 아이템을 벗어났는지 확인
            //{
            //    view.MoveCurrentToFirst();
            //}
        }

        private void foreButton_Click(object sender, RoutedEventArgs e)
        {
            //var view = GetFamilyView();
            //view.MoveCurrentToNext();
            //if (view.IsCurrentAfterLast)// 마지막 아이템을 벗어났는지 확인
            //{
            //    view.MoveCurrentToLast();
            //}
        }

        //People GetFamilyView()
        //{
        //    //People people = (People)this.FindResource("Family");
        //    //return CollectionViewSource.GetDefaultView(people);
        //}

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var index = familyList.SelectedIndex;
            if (index < 0) return;

            var item = (Person)familyList.SelectedItem;
            var age = (int)familyList.SelectedValue;

            MessageBox.Show($"선택된 {item.Name}의 나이는 {age}살 입니다.");
        }
    }

    class MainWindowViewModel 
    {
        public MainWindowViewModel()
        {
            People family = new People();
            family.Add(new Person() { Name = "Tom", Age = 11 });
            family.Add(new Person() { Name = "John", Age = 12 });
            family.Add(new Person() { Name = "Melissa", Age = 38 });
            this.Family = family;

            NamedAges namedAgeLookup = new NamedAges();
            namedAgeLookup.Add(new NamedAge() { NameForAge = "영살", AgeId = 0 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "한살", AgeId = 1 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "두살", AgeId = 2 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "세살", AgeId = 3 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "네살", AgeId = 4 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "다섯살", AgeId = 5 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "여섯살", AgeId = 6 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "일곱살", AgeId = 7 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "여덟살", AgeId = 8 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "아홉살", AgeId = 9 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "열살", AgeId = 10 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "열한살", AgeId = 11 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "열두살", AgeId = 12 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "열세살", AgeId = 13 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "열네살", AgeId = 14 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "열다섯살", AgeId = 15 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "열여섯살", AgeId = 16 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "열일곱살", AgeId = 17 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "열여덟살", AgeId = 18 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "열아홉살", AgeId = 19 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "스무살", AgeId = 20 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "서른다섯살", AgeId = 35 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "서른여섯살", AgeId = 36 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "서른일곱살", AgeId = 37 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "서른여덟살", AgeId = 38 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "서른아홉살", AgeId = 39 });
            namedAgeLookup.Add(new NamedAge() { NameForAge = "마흔살", AgeId = 40 });

            this.NamedAgeLookup = namedAgeLookup;
        }

        public People Family{ get; set; }
        public NamedAges NamedAgeLookup { get; set; }
    }

}
