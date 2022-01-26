using System;
using System.Collections.Generic;

namespace ConsoleApp
{
    class CoffeMachine : ConsoleLoop
    {
        const string exitCode = "Q";

        protected override string MenuString
        {
            get
            {
                var menu = "=============================================\n";
                menu += " 바리스타 커피머신 주메뉴\n";
                menu += "1.커피 판매       2.재료 설정         Q.종료\n";
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
            handlers.Add("1", OnCoffeeSales);
            handlers.Add("2", OnCoffeeSetting);

            OnInitialize();
        }

        void OnCoffeeSales(object sender, string menu)
        {
            Console.WriteLine($"{menu}.커피 판매 선택하였습니다.");
            new CoffeeSales().Run(this); // 커피머신을 넘김...
        }

        void OnCoffeeSetting(object sender, string menu)
        {
            Console.WriteLine($"{menu}.커피 설정 선택하였습니다.");
            new CoffeeSettings().Run(this); // 커피머신을 넘김... 
        }

        void OnInitialize()
        {
            // TODO 메인루프 내의 변수들을 초기화 한다.
        }
    }
}
