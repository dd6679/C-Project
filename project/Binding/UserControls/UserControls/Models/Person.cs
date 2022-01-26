using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserControls
{
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }


    class People : List<Person> { }
}
