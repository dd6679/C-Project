using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Library.Mvvm.Bindables;

namespace FileManager.ViewModels
{
    class CopyFileVM : MainWindowVM
    {
        #region CopyPath // 복사될 경로
        private string copyPath;
        public string CopyPath
        {
            get { return copyPath; }
            set { SetProperty(ref copyPath, value); }
        }
        #endregion

        #region CopyFileButton
        private ICommand copyFileButton;
        public ICommand CopyFileButton
        {
            get { return copyFileButton ?? (copyFileButton = new RelayCommand(Copy)); }
        }

        private void Copy()
        {
            if (File.Exists(CopyPath) == false)
            {
                File.Copy(CopyFile, CopyPath);
            }
            MessageBox.Show(CopyPath + " 복사 되었습니다.");
        }
        #endregion
    }
}
