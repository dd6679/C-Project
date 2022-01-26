using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatingGameCharacter
{
    interface IItem
    {
        void Introduce();
        void attack();
    }
    abstract class AbstractItem : IItem
    {
        public string name
        {
            get;
            protected set;
        }
        public abstract void attack();
        public abstract void Introduce();
    }
}
