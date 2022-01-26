using System;
using System.IO;
using System.Windows.Input;
using Library.Mvvm.Bindables;

namespace FileManager.ViewModels
{
    class MainWindowVM : BindableBase
    {
        string saveFile = "path.txt";

        #region 디렉토리 모델
        private Directories directoriesMember;
        public Directories DirectoriesMember
        {
            get { return directoriesMember; }
            set { SetProperty(ref directoriesMember, value); }
        }

        private DirectoriesList directoryList;
        public DirectoriesList DirectoryList
        {
            get { return directoryList; }
            set { SetProperty(ref directoryList, value); }
        }
        #endregion

        #region 파일 모델
        private Files filesMember;
        public Files FilesMember
        {
            get { return filesMember; }
            set { SetProperty(ref filesMember, value); }
        }

        private FilesList fileList;
        public FilesList FileList
        {
            get { return fileList; }
            set { SetProperty(ref fileList, value); }
        }
        #endregion

        #region CurrentPath // 현재 경로
        private string currentPath;
        public string CurrentPath
        {
            get { return currentPath; }
            set { SetProperty(ref currentPath, value); }
        }
        #endregion

        #region CopyFile // 복사할 파일
        private static string copyFile;
        public string CopyFile
        {
            get { return copyFile; }
            set { SetProperty(ref copyFile, value); }
        }
        #endregion

        #region InputPath // 현재 위치 엔터키 동작
        private ICommand inputPath;
        public ICommand InputPath
        {
            get { return inputPath ?? (inputPath = new RelayCommand(() => Manage(CurrentPath))); }
        }
        #endregion

        #region Manage // 디렉토리 리스트 추가
        private void Manage(string path)
        {
            // 임의 파일에 현재 파일 위치 저장
            Update();

            // 디렉토리 검색 후 디렉토리 리스트에 추가
            DirectoryList = new DirectoriesList();

            DirectoryInfo dInfoParent = new DirectoryInfo(path);

            try
            {
                foreach (DirectoryInfo dInfo in dInfoParent.GetDirectories())
                {
                    DirectoriesMember = new Directories();
                    DirectoriesMember.DirectoryName = dInfo.Name;
                    DirectoriesMember.DirectoryPath = dInfo.FullName;

                    path = dInfo.FullName;
                    GetSubNode(DirectoriesMember);

                    DirectoryList.Add(DirectoriesMember);
                    DirectoriesMember.Members = DirectoryList;
                }
            }
            catch (Exception)
            {
                Reset();
            }
        }
        #endregion

        #region GetSubNode // 서브 디렉토리
        private void GetSubNode(Directories dirParent)
        {
            if (dirParent == null) return;

            try
            {
                string sPath = dirParent.DirectoryPath;
                DirectoriesList SubDirectoryList = new DirectoriesList();
                DirectoryInfo dInfoParent = new DirectoryInfo(sPath);
                foreach (DirectoryInfo dInfo in dInfoParent.GetDirectories())
                {
                    Directories SubDirectoriesMember = new Directories();
                    SubDirectoriesMember.DirectoryName = dInfo.Name;
                    SubDirectoriesMember.DirectoryPath = dInfo.FullName;
                    SubDirectoryList.Add(SubDirectoriesMember);
                    GetSubNode(SubDirectoriesMember);
                }
                dirParent.Members = SubDirectoryList;
                
            }
            catch (Exception)
            {
                Reset();
            }
        }
        #endregion

        #region DisplayFile // 디렉토리 선택
        private ICommand displayFile;
        public ICommand DisplayFile
        {
            get { return displayFile ?? (displayFile = new RelayCommand<Directories>(Display)); }
        }
        #endregion

        #region Display // 파일 리스트에 추가
        private void Display(Directories directoryName)
        {
            if (directoryName == null)
                return;
            try
            {
                CurrentPath = directoryName.DirectoryPath;
                Update();
                FileList = new FilesList();
                string[] files = Directory.GetFiles(CurrentPath);
                foreach (string s in files)
                {
                    FilesMember = new Files();
                    FilesMember.FileName = new FileInfo(s).Name;
                    FilesMember.FilePath = s;
                    FileList.Add(FilesMember);
                }
            }
            catch (Exception)
            {
                Reset();
            }
        }
        #endregion

        #region SelectFile // 파일 선택
        private ICommand selectFile;
        public ICommand SelectFile
        {
            get { return selectFile ?? (selectFile = new RelayCommand<Files>(Select)); }
        }
        #endregion

        #region Select // 카피 파일에 추가
        private void Select(Files fileName)
        {
            if (fileName == null)
                return;
            CopyFile = fileName.FilePath;
        }
        #endregion

        #region Reset // 마지막에 저장된 위치로 초기화
        public void Reset()
        {
            //var fileInfo = new FileInfo(savePath);
            var savePath = System.Environment.CurrentDirectory + @"\" +saveFile;
            // 파일 존재 여부 예외 처리
            if (File.Exists(saveFile))
            {
                using (StreamReader sr = new StreamReader(saveFile))
                {
                    var path = sr.ReadLine();
                    if (!string.IsNullOrEmpty(path))
                    {
                        this.CurrentPath = path;
                    }
                }
            }
            //else
            //{
            //    FileStream fs = fileInfo.Create();
            //}
        }
        #endregion

        #region Update // 임의 파일에 현재 위치 업데이트
        private void Update()
        {
            using (StreamWriter wr = new StreamWriter(saveFile))
            {
                wr.WriteLine(CurrentPath);
            }
        }
        #endregion

        #region 생성자
        public MainWindowVM()
        {
            CurrentPath = @"D:\";

            Reset();

            Manage(CurrentPath);
            //Directory.GetDirectories(CurrentPath); //????
        }
        #endregion
    }
}