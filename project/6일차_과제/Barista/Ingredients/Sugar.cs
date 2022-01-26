using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barista.Ingredients
{
    class Sugar : AbstractIngredient
    {
        public Sugar()
        {
            this.price = 500;
        }
        public override string Name
        {
            get
            {
                return "설탕/시럽";
            }
        }
    }
}
