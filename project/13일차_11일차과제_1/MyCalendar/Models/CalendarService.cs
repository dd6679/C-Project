using System;
using System.Collections.Generic;

namespace MyCalendar
{
    public class CalendarService
    {
        #region 프로퍼티
        public int Month { get; private set; }
        public int Year { get; private set; }
        public int StartDateOfMonth { get; private set; }
        public int LastDayOfMonth { get; private set; }
        #endregion

        #region 생성자 // 연,월을 제공받음.
        public CalendarService(int year, int month)
        {
            this.Year = year;
            this.Month = month;

            var date = new DateTime(this.Year, this.Month, 1);
            this.StartDateOfMonth = (int)date.AddDays(-date.Day + 1).DayOfWeek;
            this.LastDayOfMonth = DateTime.DaysInMonth(date.Year, date.Month);
        }
        #endregion

        #region Dates // 해당 년,월의 날짜리스트 반환
        public List<int> Dates
        {
            get
            {
                var dates = new List<int>();
                for (int i = 1; i <= this.LastDayOfMonth; i++)
                {
                    dates.Add(i);
                }
                return dates;
            }
        }
        #endregion
    }
}
