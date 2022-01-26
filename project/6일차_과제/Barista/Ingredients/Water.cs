using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barista.Ingredients
{
    class Water : AbstractIngredient
    {
        public Water()
        {
            this.price = 1000;
        }
        public override string Name
        {
            get
            {
                return "물";
            }
        }
    }
}
