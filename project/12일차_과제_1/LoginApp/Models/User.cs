using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Mvvm.Bindables;

namespace LoginApp
{
    class User// : BindableBase
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
