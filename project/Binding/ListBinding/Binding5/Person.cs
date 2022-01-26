using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binding1
{
    //class Person : INotifyPropertyChanged
    //{
    //    public event PropertyChangedEventHandler PropertyChanged;
    //    protected void Notify(string propName)
    //    {
    //        if (this.PropertyChanged != null)
    //        {
    //            PropertyChanged(this, new PropertyChangedEventArgs(propName));
    //        }
    //    }

    //    string name;
    //    public string Name
    //    {
    //        get { return this.name; }
    //        set
    //        {
    //            if (this.name == value)
    //                return;

    //            this.name = value;
    //            Notify("Name");
    //        }
    //    }

    //    int age;
    //    public int Age
    //    {
    //        get { return this.age; }
    //        set
    //        {
    //            if (this.age == value)
    //                return;

    //            this.age = value;
    //            Notify("Age");
    //        }
    //    }
    //}

    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }


    class People : List<Person>{ }

}
