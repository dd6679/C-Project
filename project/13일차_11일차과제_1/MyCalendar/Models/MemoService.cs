using System.Collections.Generic;

namespace MyCalendar
{
    public class MemoService
    {
        private Dictionary<string, List<string>> _memoDictionary = new Dictionary<string, List<string>>();

        #region Add // 키와 리스트를 받는 버전
        public void Add(string key, List<string> memos)
        {
            this[key] = memos;
        }
        #endregion

        #region Add // 키와 문자열을 받는 버전
        public void Add(string key, string memo)
        {
            var memos = this[key];
            if (memos != null)
            {
                memos.Add(memo);
            }
            else
            {
                memos = new List<string>();
                memos.Add(memo);
            }

            Add(key, memos);
        }
        #endregion

        #region 인덱서 // 키에 리스트를 연결시, Add 메서드의 키와 리스트를 받는버전과 동일
        public List<string> this[string key]
        {
            get
            {
                List<string> result = null;
                if ( !string.IsNullOrEmpty(key) && _memoDictionary.ContainsKey(key)) // 키가 정상이고 데이터가 있을 때 리턴.
                {
                    result = _memoDictionary[key];
                }

                return result;
            }
            set
            {
                if (string.IsNullOrEmpty(key)) // 키가 비정상이면 아무것도 하지않음.
                    return;

                if (!_memoDictionary.ContainsKey(key))
                {
                    _memoDictionary[key] = value;
                }
            }
        }
        #endregion

        #region MakeKey // 키조합 알고리즘 (지금은 단순연결)
        public static string MakeKey(string year, string month, string date)
        {
            return year + month + date; // 키는 년월일 단순조합.
        }
        #endregion
    }
}
