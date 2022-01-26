using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barista.Ingredients
{
    class Espresso : AbstractIngredient
    {
        public Espresso()
        {
            this.price = 2000;
        }
        public override string Name
        {
            get
            {
                return "에스프레소 커피";
            }
        }
    }
}
