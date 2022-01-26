using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using LoginApp.ViewModel;
using MyCalendar.Util;

namespace LoginApp
{
    class MainWindowVM : ViewModelBase
    {

        // 로그인에서 접속버튼
        private ICommand connectButton;
        public ICommand ConnectButton
        {
            get { return connectButton ?? (connectButton = new RelayCommand(PopUp)); }
        }

        // 로그인에서 사용자 ID와 비밀번호
        private string loginID, loginPassword;
        public string LoginID
        {
            get { return loginID; }
            set { SetProperty(ref loginID, value); }
        }
        public string LoginPassword
        {
            get { return loginPassword; }
            set { SetProperty(ref loginPassword, value); }
        }

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
    }
}
