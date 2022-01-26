using System;
using System.Collections.Generic;

namespace ConsoleApp
{
    class CoffeeSales : ConsoleLoop
    {
        const string exitCode = "Q";
        protected override string MenuString
        {
            get
            {
                var menu = "=============================================\n";
                menu += " 바리스타 커피 판매 메뉴\n";
                menu += "1.커피   2.물     3.우유    4.얼음     Q.종료\n";
                menu += "=============================================\n";

                return menu;
            }
        }

        protected override bool IsExit(string selected)
        {
            return selected == exitCode;
        }
        protected override void RegisterSubMenu(Dictionary<string, ProcesHandler> handlers)
        {
            handlers.Add("1", OnSelectedCoffee);
            handlers.Add("2", OnSelectedWater);
            handlers.Add("3", OnSelectedMilk);
            handlers.Add("4", OnSelectedIce);
        }

        void OnSelectedCoffee(object sender, string menu)
        {
            var coffeeMachine = sender as CoffeMachine;

            Console.WriteLine($"{menu}.커피를 선택하였습니다.");
            Console.ReadLine();
        }

        void OnSelectedWater(object sender, string menu)
        {
            var coffeeMachine = sender as CoffeMachine;

            Console.WriteLine($"{menu}.물을 선택하였습니다.");
            Console.ReadLine();
        }

        void OnSelectedMilk(object sender, string menu)
        {
            var coffeeMachine = sender as CoffeMachine;

            Console.WriteLine($"{menu}.우유를 선택하였습니다.");
            Console.ReadLine();
        }

        void OnSelectedIce(object sender, string menu)
        {
            var coffeeMachine = sender as CoffeMachine;

            Console.WriteLine($"{menu}.얼음을 선택하였습니다.");
            Console.ReadLine();
        }
    }
}
