using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; // ObservableCollection
using System.Diagnostics;             // WriteLine
using System.Windows.Input;
using Library.Mvvm.Bindables;

namespace MyCalendar
{
    public class MainWindowVM : BindableBase
    {
        // 메세지
        #region Messages
        const string msgInputYear = "년도를 입력하세요.";
        const string msgInputMonth = "달을 입력하세요.";
        const string msgInputMemo = "메모를 입력하세요.";
        #endregion

        #region MemoService // 메모 관리자.
        private MemoService _memoService = new MemoService(); // VM 생성시 같이 생성
        #endregion

        // 달력 프로퍼티
        #region Year // 입력한 년도
        private string _year = msgInputYear;
        public string Year
        {
            get { return _year; }
            set { this.SetProperty(ref _year, value); }
        }
        #endregion

        #region Month // 입력한 월
        private string _month = msgInputMonth;
        public string Month
        {
            get { return _month; }
            set { this.SetProperty(ref _month, value); }
        }
        #endregion

        #region SelectedDate // 선택된 날짜
        private string _selectedDateDate;
        public string SelectedDate
        {
            get { return _selectedDateDate; }
            set
            {
                if (this.SetProperty(ref _selectedDateDate, value))
                {
                    SelectButtonClick(value);
                }
            }
        }
        #endregion

        #region FirstDay // 1일이 되는 요일
        private int firstday;
        public int FirstDay
        {
            get { return firstday; }
            set { this.SetProperty(ref firstday, value); }
        }
        #endregion

        // 메모 프로퍼티
        #region DateForMemo// 메모에 대한 날짜
        private List<int> _dateForMemo;
        public List<int> DateForMemo
        {
            get { return _dateForMemo; }
            set { this.SetProperty(ref _dateForMemo, value); }
        }
        #endregion

        #region MemoForDate // 날짜에 대한 메모
        private ObservableCollection<string> _memoForDate;
        public ObservableCollection<string> MemoForDate
        {
            get { return _memoForDate; }
            set { this.SetProperty(ref _memoForDate, value); }
        }
        #endregion

        #region Memo // 메모 입력
        private string memo = msgInputMemo;
        public string Memo
        {
            get { return memo; }
            set { this.SetProperty(ref memo, value); }
        }
        #endregion
               
        // 커맨드
        #region InputCommand // 년도, 월 입력 후 입력 버튼
        private ICommand inputCommand;
        public ICommand InputCommand
        {
            get
            {
                return this.inputCommand ?? (this.inputCommand = new RelayCommand(InputButtonClick, CanExecuteInputButtonClick));
            }
        }

        private bool CanExecuteInputButtonClick()
        {
            return ( _year != msgInputYear 
                   && _month != msgInputMonth 
                   && !string.IsNullOrEmpty(_year) 
                   && !string.IsNullOrEmpty(_month) );
        }

        private void InputButtonClick()
        {
            try
            {
                DisplayDate(int.Parse(Year), int.Parse(Month));                
            }
            catch (Exception)
            {
                // 입력형식 팝업
            }
        }

        private void DisplayDate(int year, int month)
        {
            // 달력 관리자
            var calendarService = new CalendarService(int.Parse(Year), int.Parse(Month));
            this.DateForMemo = calendarService.Dates;
            this.FirstDay = calendarService.StartDateOfMonth;
        }
        #endregion
               
        #region SelectCommand // 달력에서 날짜 선택
        private void SelectButtonClick(string date)
        {
            try
            {
                if (date != null)
                {
                    var key = MemoService.MakeKey(this.Year, this.Month, date);
                    var value = _memoService[key];
                    this.MemoForDate = value == null ? null : new ObservableCollection<string>(_memoService[key]);
                }
            }
            catch (Exception ex)
            {
                this.MemoForDate = null;
                Debug.WriteLine(ex.Message);
            }
        }
        #endregion
        
        #region SaveCommand  // 메모 저장 버튼
        private ICommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return this.saveCommand ?? (this.saveCommand = new RelayCommand(SaveButtonClick, CanExecuteSaveButtonClick));
            }
        }

        private bool CanExecuteSaveButtonClick()
        {
            return ( !string.IsNullOrEmpty(SelectedDate) 
                    && !string.IsNullOrEmpty(this.Memo)
                    && memo != msgInputMemo);
        }

        private void SaveButtonClick()
        {
            try
            {
                var key = MemoService.MakeKey(Year, Month, SelectedDate);
                _memoService.Add(key, this.Memo); // 메모를 직접 넣는버전

                this.MemoForDate = new ObservableCollection<string>(_memoService[key]);
                this.Memo = string.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
        #endregion
    }
}
