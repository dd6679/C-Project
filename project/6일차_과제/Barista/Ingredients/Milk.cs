using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barista.Ingredients
{
    class Milk : AbstractIngredient
    {
        public Milk()
        {
            this.price = 1200;
        }
        public override string Name
        {
            get
            {
                return "우유";
            }
        }
    }
}
