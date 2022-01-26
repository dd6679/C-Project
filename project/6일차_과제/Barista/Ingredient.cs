using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barista
{
    interface Ingredient
    {
        string Name
        {
            get;
        }
        int Price
        {
            get; set;
        }
    }

    abstract class AbstractIngredient : Ingredient
    {
        protected int price;
        public abstract string Name { get; }
        public int Price
        {
            get
            {
                return this.price;
            }
            set
            {
                this.price = value;
            }
        }
    }
}

