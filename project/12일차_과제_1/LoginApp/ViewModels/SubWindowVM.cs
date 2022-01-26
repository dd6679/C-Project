using System.Windows;
using System.Windows.Input;
using LoginApp.ViewModel;
using Library.Mvvm.Bindables;

namespace LoginApp
{
    class SubWindowVM : ViewModelBase
    {
        #region ID
        string _id = string.Empty;
        public string ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }
        #endregion

        #region Name
        string _name = string.Empty;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        #endregion

        #region Password
        string _password = string.Empty;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
        #endregion

        #region Confirm
        string _confirm = string.Empty;
        public string Confirm
        {
            get { return _confirm; }
            set { SetProperty(ref _confirm, value); }
        }
        #endregion

        #region EnrollButton
        // 회원가입에서 등록버튼
        private ICommand enrollButton;
        public ICommand EnrollButton
        {
            get { return enrollButton ?? (enrollButton = new RelayCommand(Enroll, CanEnroll)); }
        }

        private bool CanEnroll()
        {
            return  (!string.IsNullOrEmpty(this.ID) &&
                    !string.IsNullOrEmpty(this.Name)&&
                    !string.IsNullOrEmpty(this.Password) &&
                    !string.IsNullOrEmpty(this.Confirm) &&
                    this.Password == this.Confirm);
        }

        // 딕셔너리에 유저 추가
        private void Enroll()
        {
            if (dictionary.ContainsKey(this.ID) == false)
            {
                var newUser = new User() { ID = ID, Name = Name, Password = Password };
                dictionary.Add(this.ID, newUser);
                MessageBox.Show($"{Name}님 가입되었습니다.", "가입 완료");

                this.ID = "";
                this.Name = "";
                this.Password = "";
                this.Confirm = "";
            }
            else
            {
                MessageBox.Show($"{Name} 은 이미 가입되어있습니다.", "warning");
            }
        }
        #endregion
    }
}



