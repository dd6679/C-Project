using System.Windows;
using System.Windows.Input;
using LoginApp.ViewModel;
using Library.Mvvm.Bindables;

namespace LoginApp
{
    class MainWindowVM : ViewModelBase
    {
        #region ConnectButton
        // 로그인에서 접속버튼
        private ICommand connectButton;
        public ICommand ConnectButton
        {
            get { return connectButton ?? (connectButton = new RelayCommand(PopUp,CanExecutePopup)); }
        }

        private bool CanExecutePopup()
        {
            return (!string.IsNullOrEmpty(Host) &&
                    !string.IsNullOrEmpty(LoginID) &&
                    !string.IsNullOrEmpty(LoginPassword));
        }
        #endregion

        #region Host
        // 로그인에서 사용자 ID와 비밀번호
        private string host;
        public string Host
        {
            get { return host; }
            set { SetProperty(ref host, value); }
        }
        #endregion

        #region LoginID
        private string loginID;
        public string LoginID
        {
            get { return loginID; }
            set { SetProperty(ref loginID, value); }
        }
        #endregion

        #region LoginPassword
        private string loginPassword;
        public string LoginPassword
        {
            get { return loginPassword; }
            set { SetProperty(ref loginPassword, value); }
        }
        #endregion

        #region PopUp
        // 팝업 표출
        // 회원가입한 유저와 비교
        private void PopUp()
        {
            if (LoginID != null && dictionary.ContainsKey(LoginID))
            {
                if (dictionary[LoginID].Password == LoginPassword)
                {
                    MessageBox.Show($"{dictionary[LoginID].Name}님 성공적으로 접속하였습니다.", "접속 완료");
                }
                else
                {
                    MessageBox.Show("접속에 실패하였습니다.", "접속 실패");
                }
            }
            else
            {
                MessageBox.Show("접속에 실패하였습니다.", "접속 실패");
            }
        }
        #endregion
    }
}
