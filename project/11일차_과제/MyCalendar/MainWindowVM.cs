using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MyCalendar.Util;

namespace MyCalendar
{
    public class MainWindowVM : BindableBase
    {
        Dictionary<string, List<string>> dictionary;
        List<string> list;

        public MainWindowVM()
        {
            MemoForDate = new ObservableCollection<string>();
            dictionary = new Dictionary<string, List<string>>();
            list = new List<string>();

        }

        #region ObservableCollection
        ObservableCollection<int> dateForMemo;
        ObservableCollection<string> memoForDate;

        // 메모에 대한 날짜
        public ObservableCollection<int> DateForMemo
        {   
            get { return dateForMemo; }
            set { this.SetProperty(ref dateForMemo, value); }
        }

        // 날짜에 대한 메모
        public ObservableCollection<string> MemoForDate
        {
            get { return memoForDate; }
            set { this.SetProperty(ref memoForDate, value); }
        }
        #endregion

        string year, month, date;
        public string Year
        {
            get { return year; }
            set { this.SetProperty(ref year, value); }
        }

        public string Month
        {
            get { return month; }
            set { this.SetProperty(ref month, value); }
        }

        public string Date
        {
            get { return date; }
            set { this.SetProperty(ref date, value); }
        }

        // 1일이 되는 요일
        private int firstday;
        public int FirstDay
        {
            get { return firstday; }
            set { this.SetProperty(ref firstday, value); }
        }

        private string memo;
        public string Memo
        {
            get { return memo; }
            set { this.SetProperty(ref memo, value); }
        }

        // 년도, 월 입력 후 입력 버튼
        #region InputCommand
        private ICommand inputCommand;
        public ICommand InputCommand
        {
            get
            {
                return this.inputCommand ?? (this.inputCommand = new RelayCommand(InputButtonClick));
            }
        }

        private void InputButtonClick()
        {
            try
            {
                DisplayDate(int.Parse(Year), int.Parse(Month));
            }
            catch (Exception)
            {

            }
        }
        #endregion

        // 달력에서 날짜 선택
        #region SelectCommand
        private ICommand selectCommand;
        public ICommand SelectCommand
        {
            get
            {
                return this.selectCommand ?? (this.selectCommand = new RelayCommand(SelectButtonClick));
            }
        }
        

        private void SelectButtonClick(/*string s*/)
        {
            /*int i = int.Parse(s);*/
            // 일 저장
            int i = 0; // 선택한 날짜를 가져오는 것을 아직 구현하지 못했습니다.
            Date = DateForMemo[i].ToString();
            // 기존 메모 보여줌
            List<string> list;
            string key = Year + Month + Date;
            MemoForDate.Clear();
            if (dictionary.TryGetValue(key, out list))
            {
                foreach (string l in list)
                {
                    MemoForDate.Add(l);
                }
            }
        }
        #endregion

        // 메모 저장 버튼
        #region SaveCommand
        private ICommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return this.saveCommand ?? (this.saveCommand = new RelayCommand(SaveButtonClick));
            }
        }

        private void SaveButtonClick()
        {
            // 메모 저장
            string key = Year + Month + Date;
            List<string> list;
            if(dictionary.TryGetValue(key, out list) == false)
            {
                list = new List<string>();
                list.Add(Memo);
                dictionary.Add(key, list);
            }
            MemoForDate.Add(Memo);
        }
        #endregion
        private void DisplayDate(int year, int month)
        {
            DateTime date = new DateTime(year, month, 1);
            FirstDay = (int)date.AddDays(-date.Day + 1).DayOfWeek;
            var lastday = DateTime.DaysInMonth(date.Year, date.Month);

            DateForMemo = new ObservableCollection<int>();
            for (int i = 1; i <= lastday; i++)
            {
                DateForMemo.Add(i);
            }
        }

    }


}
