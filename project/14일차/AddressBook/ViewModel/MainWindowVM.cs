using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Library.Mvvm.Bindables;

namespace AddressBook
{
    class MainWindowVM : BindableBase
    {
        // 작업시간 3시간 반
        public Dictionary<string, Member> dictionary;

        #region Name
        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
        #endregion

        #region Age
        private int age;
        public int Age
        {
            get { return age; }
            set { SetProperty(ref age, value); }
        }
        #endregion

        #region Address
        private string address;
        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }
        #endregion

        #region PhoneNumber
        private string phoneNumber;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { SetProperty(ref phoneNumber, value); }
        }
        #endregion

        #region Information
        private string information;
        public string Information
        {
            get { return information; }
            set { SetProperty(ref information, value); }
        }
        #endregion

        #region SearchName // 검색키워드
        private string searchName;
        public string SearchName
        {
            get { return searchName; }
            set { SetProperty(ref searchName, value); }
        }
        #endregion

        #region MemberList
        private ObservableCollection<Member> memberList;
        public ObservableCollection<Member> MemberList
        {
            get { return memberList; }
            set { SetProperty(ref memberList, value); }
        }
        #endregion

        #region 생성자
        public MainWindowVM()
        {
            dictionary = new Dictionary<string, Member>();
            MemberList = new ObservableCollection<Member>();

            // 파일 읽어 이전 저장 내용 로딩
            using (StreamReader rdr = new StreamReader(@"C:\Users\user\Desktop\data.txt"))
            {
                string line;
                while ((line = rdr.ReadLine()) != null)
                {
                    string[] memberInfo = line.Split('/');

                    var newMember = new Member() { Name = memberInfo[0], Age = int.Parse(memberInfo[1]), Address = memberInfo[2], PhoneNumber = memberInfo[3], Information = memberInfo[4] };
                    dictionary.Add(memberInfo[0], newMember);
                    MemberList.Add(newMember);
                }
            }
        }
        #endregion

        #region SaveButton // 저장 버튼
        private ICommand saveButton;
        public ICommand SaveButton
        {
            get { return saveButton ?? (saveButton = new RelayCommand(Save, CanExecuteSave)); }
        }
        private bool CanExecuteSave()
        {
            return !string.IsNullOrEmpty(Name) &&
                    !string.IsNullOrEmpty(Age.ToString()) &&
                    !string.IsNullOrEmpty(Address) &&
                    !string.IsNullOrEmpty(PhoneNumber) &&
                    !string.IsNullOrEmpty(Information);
        }
        private void Save()
        {
            // 딕셔너리에 이름이 없을 때 새로 저장
            if (dictionary.ContainsKey(Name) == false)
            {
                var newMember = new Member() { Name = Name, Age = Age, Address = Address, PhoneNumber = PhoneNumber, Information = Information };
                dictionary.Add(Name, newMember);
                MemberList.Add(newMember);
                MessageBox.Show($"{Name}님 저장되었습니다.", "저장 완료");
            }
            else // 이름이 있다면 수정
            {
                dictionary[Name] = new Member() { Name = Name, Age = Age, Address = Address, PhoneNumber = PhoneNumber, Information = Information };
                MessageBox.Show($"{Name}님 수정되었습니다.", "저장 완료");
            }
            // 구분자 /를 이용해 파일에 쓰기
            using (StreamWriter wr = new StreamWriter(@"C:\Users\user\Desktop\data.txt"))
            {
                foreach (Member m in dictionary.Values)
                {
                    wr.WriteLine(m.Name + "/" + m.Age + "/" + m.Address + "/" + m.PhoneNumber + "/" + m.Information);
                }
            }
            //입력 항목 클리어
            Name = "";
            Age = 0;
            Address = "";
            PhoneNumber = "";
            Information = "";
        }
        #endregion

        #region SelectedChanged // 리스트박스 항목 선택
        private ICommand selectedChanged;
        public ICommand SelectedChanged
        {
            get { return selectedChanged ?? (selectedChanged = new RelayCommand<Member>(Marked)); }
        }
        #endregion

        #region Marked // 각 항목에 내용 표시
        private void Marked(Member member)
        {
            Name = member.Name;
            Age = dictionary[Name].Age;
            Address = dictionary[Name].Address;
            PhoneNumber = dictionary[Name].PhoneNumber;
            Information = dictionary[Name].Information;
        }
        #endregion

        #region SearchButton // 검색 버튼
        private ICommand searchButton;
        public ICommand SearchButton
        {
            get { return searchButton ?? (searchButton = new RelayCommand(Search)); }
        }
        private void Search()
        {
            if (dictionary.ContainsKey(SearchName))
            {
                Marked(dictionary[SearchName]);
            }
            else
            {
                MessageBox.Show("찾지 못하였습니다.");
            }
        }
        #endregion
    }
}
