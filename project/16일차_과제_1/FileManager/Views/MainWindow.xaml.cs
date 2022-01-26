using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FileManager
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

        private void CopyFile_Click(object sender, RoutedEventArgs e)
        {
            CopyFile copyFile = new CopyFile();
            copyFile.Owner = this;
            copyFile.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            copyFile.ShowDialog();
        }

        private void CreateDirectory_Click(object sender, RoutedEventArgs e)
        {
            CreateDirectory createDirectory = new CreateDirectory();
            createDirectory.Owner = this;
            createDirectory.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            createDirectory.ShowDialog();
        }

        private void Window_Closing(object sender, RoutedEventArgs e)
        {
            base.Close();
        }

        /*string CurrentPath = @"C:\";
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                CurrentPath = xTextBox.Text;
                Window_Loaded(sender, e);
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            xDirectoryList.Items.Clear();
            DirectoryInfo dInfoParent = new DirectoryInfo(CurrentPath);
            foreach (DirectoryInfo dInfo in dInfoParent.GetDirectories())   // 특정폴더
            {
                try
                {
                    TreeViewItem item = new TreeViewItem();
                    item.Header = dInfo.Name;
                    item.Tag = dInfo.FullName;
                    item.Expanded += new RoutedEventHandler(item_Expanded);   // 노드 확장시 추가

                    xDirectoryList.Items.Add(item);
                    GetSubDirectories(item);
                }

                catch (Exception except)
                {
                    // MessageBox.Show(except.Message);   // 접근 거부 폴더로 인해 주석처리
                }
            }
        }

        // 서브 디렉토리
        private void GetSubDirectories(TreeViewItem itemParent)
        {
            if (itemParent == null) return;
            if (itemParent.Items.Count != 0) return;

            try
            {
                string strPath = itemParent.Tag as string;
                DirectoryInfo dInfoParent = new DirectoryInfo(strPath);
                foreach (DirectoryInfo dInfo in dInfoParent.GetDirectories())
                {
                    TreeViewItem item = new TreeViewItem();
                    item.Header = dInfo.Name;
                    item.Tag = dInfo.FullName;
                    item.Expanded += new RoutedEventHandler(item_Expanded);
                    itemParent.Items.Add(item);
                }
            }

            catch (Exception except)
            {
                // MessageBox.Show(except.Message);   // 접근 거부 폴더로 인해 주석처리
            }
        }

        // 트리확장시 내용 추가
        void item_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem itemParent = sender as TreeViewItem;
            if (itemParent == null) return;
            if (itemParent.Items.Count == 0) return;
            foreach (TreeViewItem item in itemParent.Items)
            {
                GetSubDirectories(item);
            }
        }*/
    }
}
