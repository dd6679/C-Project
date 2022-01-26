using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barista
{
    class Program
    {
        static void Main(string[] args)
        {
            //작업시간: 4시간
            var coffeeMachine = new CoffeeMachine();
            coffeeMachine.MainMenuLoop();
        }
    }
}
