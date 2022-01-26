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
    }
}
