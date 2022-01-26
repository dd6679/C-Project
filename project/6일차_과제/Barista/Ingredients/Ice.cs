using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barista.Ingredients
{
    class Ice : AbstractIngredient
    {
        public Ice()
        {
            this.price = 700;
        }
        public override string Name
        {
            get
            {
                return "얼음";
            }
        }
    }
}
