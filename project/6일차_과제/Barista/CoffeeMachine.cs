using System;
using System.Collections;
using Barista.Ingredients;

namespace Barista
{
    delegate void MenuDelegate();
    class CoffeeMachine : App
    {
        // 주메뉴, 부메뉴 선택숫자를 키로 하는 해시테이블
        Hashtable main_ht = new Hashtable();
        Hashtable sub_ht = new Hashtable();
        
        // 주메뉴 1, 2 분기를 위해 사용
        MenuDelegate SalesMenu, SettingMenu;

        private AbstractIngredient selectedItem;
        private int totalPrice;
        private string totalItem;

        // 생성 시 부메뉴 해시테이블 설정
        public CoffeeMachine()
        {
            sub_ht["1"] = new Espresso();
            sub_ht["2"] = new Water();
            sub_ht["3"] = new Milk();
            sub_ht["4"] = new Sugar();
            sub_ht["5"] = new Ice();
        }
        
        // 부메뉴 아이템 선택 시
        public void SelectItem(string no)
        {
            this.selectedItem = (AbstractIngredient)sub_ht[no];
        }

        // 주메뉴 - 2. 재료 가격 설정
        private void UpdatePrice()
        {
            if (selectedItem != null)
            {
                string input = ShowAddtionalMenu($"새로운 가격을 입력하세요(현재 가격 : {selectedItem.Price}) : ");

                try
                {
                    var price = int.Parse(input);
                    this.selectedItem.Price = price;

                    Console.WriteLine($"{selectedItem.Name} 가격이 {selectedItem.Price} 원으로 업데이트 되었습니다.");
                }
                catch (Exception)
                {
                    Console.WriteLine($"잘못된 값이 입력되었습니다. : {input}");
                }
            }
        }

        // 주메뉴 - 1. 커피 판매 시 재료 추가
        private void AddSales()
        {
            if (selectedItem != null)
            {
                this.totalPrice += selectedItem.Price;
                this.totalItem += selectedItem.Name + " ";
                Console.WriteLine($"{selectedItem.Name} 이(가) 추가되었습니다.");
            }
        }

        // 주메뉴 - 1. 커피 판매 시 가격 출력 및 초기화
        private void PrintSales()
        {
            Console.WriteLine($"선택한 커피 재료는 {totalItem}이고 가격은 {totalPrice}원 입니다.");

            this.totalPrice = 0;
            this.totalItem = string.Empty;
        }

        // 각 델리게이트 설정
        private void SettingDelegate()
        {
            SalesMenu = new MenuDelegate(AddSales);
            SettingMenu = new MenuDelegate(UpdatePrice);
        }

        // 주메뉴 루프
        public void MainMenuLoop()
        {
            while (true)
            {
                string selected = ShowMenu("주 메뉴\n1. 커피 판매, 2. 재료 가격 설정, exit. 종료");
                if (selected == "exit")
                    break;

                SubMenuLoop(selected);
            }
        }

        // 부메뉴 루프
        private void SubMenuLoop(string mainMenu)
        {
            string selected = "";

            while (selected != "Q")
            {
                selected = ShowMenu("부 메뉴\n1. 에스프레소 커피, 2. 물, 3. 우유, 4. 설탕/시럽, 5. 얼음, Q. 주메뉴");

                SelectItem(selected);

                SettingDelegate();

                // 엔터 쳤을 때 총 재료 및 가격 표시
                if (selected == "")
                {
                    SalesMenu -= new MenuDelegate(AddSales);
                    SalesMenu += new MenuDelegate(PrintSales);
                }

                main_ht["1"] = SalesMenu;
                main_ht["2"] = SettingMenu;

                var MyDelegate = (MenuDelegate)main_ht[mainMenu];

                MyDelegate();

                Console.ReadLine();
            }
        }
    }
}
