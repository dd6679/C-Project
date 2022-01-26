using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBinding
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Traits Traits { get; set; }
    }

    public class Trait
    {
        public string Description { get; set; }
    }

    public class Traits : List<Trait>{ }



    public class People : List<Person> { }

    public class Family
    {
        public string FamilyName { get; set; }
        public People Members { get; set; }
    }

    public class Families : List<Family> { }

}
