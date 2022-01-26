using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LoginApp.ViewModel;
using MyCalendar.Util;

namespace LoginApp
{
    class SubWindowVM : ViewModelBase
    {
        public SubWindowVM()
        {
            SelectedModel = new User();
        }
        // 회원가입에서 등록버튼
        private ICommand enrollButton;
        public ICommand EnrollButton
        {
            get { return enrollButton ?? (enrollButton = new RelayCommand(Enroll)); }
        }

        // 딕셔너리에 유저 추가
        private void Enroll()
        {
            if (dictionary.ContainsKey(SelectedModel.ID) == false && SelectedModel.Password == SelectedModel.VPassword)
            {
                dictionary.Add(SelectedModel.ID, SelectedModel);
                MessageBox.Show($"{SelectedModel.Name}님 가입되었습니다.", "가입 완료");
            }

            SelectedModel = null;
            SelectedModel = new User();
        }
    }
}
