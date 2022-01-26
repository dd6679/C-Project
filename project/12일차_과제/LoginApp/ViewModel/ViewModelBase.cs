using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCalendar.Util;

namespace LoginApp.ViewModel
{
    class ViewModelBase : BindableBase
    {
        static public Dictionary<string, User> dictionary;

        // 사용자 정의
        private User selectedModel;
        public User SelectedModel
        {
            get { return selectedModel; }
            set { SetProperty(ref selectedModel, value); }
        }

        public ViewModelBase()
        {
            dictionary = new Dictionary<string, User>();
        }
    }
}
