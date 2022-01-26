using System.Collections.Generic;
using Library.Mvvm.Bindables;

namespace LoginApp.ViewModel
{
    class ViewModelBase : BindableBase
    {
        static public Dictionary<string, User> dictionary;

        public ViewModelBase()
        {
            dictionary = new Dictionary<string, User>();
        }
    }
}
