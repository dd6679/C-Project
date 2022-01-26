using System.IO;
using System.Windows;
using System.Windows.Input;
using Library.Mvvm.Bindables;

namespace FileManager.ViewModels
{
    class CreateDirectoryVM : MainWindowVM
    {
        #region DirectoryName // 생성할 디렉토리 이름
        private string directoryName;
        public string DirectoryName
        {
            get { return directoryName; }
            set { SetProperty(ref directoryName, value); }
        }
        #endregion

        #region CreateDirectoryButton
        private ICommand createDirectoryButton;
        public ICommand CreateDirectoryButton
        {
            get { return createDirectoryButton ?? (createDirectoryButton = new RelayCommand(Create)); }
        }
        private void Create()
        {
            if (Directory.Exists(Path.Combine(CurrentPath, DirectoryName)) == false)
            {
                Directory.CreateDirectory(Path.Combine(CurrentPath, DirectoryName));
            }
            MessageBox.Show(Path.Combine(CurrentPath, DirectoryName) + " 생성 되었습니다.");
        }
        #endregion
    }
}
