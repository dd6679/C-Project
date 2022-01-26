using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCalendar.Util;

namespace LoginApp
{
    class User : BindableBase
    {
        private string id, name, password, vpassword;

        public string ID
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }
        public string VPassword
        {
            get { return vpassword; }
            set { SetProperty(ref vpassword, value); }
        }
    }
}
