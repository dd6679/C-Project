using System;

namespace Barista
{
    class Ingredient
    {
        public string Name { get; set; }
        public int Price { get; set; }

        public Ingredient(string name, int price)
        {
            this.Name = name;
            this.Price = price;
        }

        public void Update(int price)
        {
            this.Price = price;
        }
    }

    class Espresso : Ingredient
    {
        public Espresso()
            : base("에스프레소 커피", 2000)
        {
        }
    }

    class Water : Ingredient
    {
        public Water()
            : base("물", 1000)
        {
        }
    }

    class Milk : Ingredient
    {
        public Milk()
             : base("우유", 1200)
        {
        }
    }

    class Sugar : Ingredient
    {
        public Sugar()
             : base("설탕/시럽", 500)
        {
        }
    }

    class Ice : Ingredient
    {
        public Ice()
            : base("얼음", 700)
        {
        }
    }

    class CoffeeMachine
    {
        private Espresso espresso;
        private Water water;
        private Milk milk;
        private Sugar sugar;
        private Ice ice;

        private Ingredient selectedItem;
        private int totalPrice;
        private string totalItem;

        public CoffeeMachine()
        {
            this.espresso = new Espresso();
            this.water = new Water();
            this.milk = new Milk();
            this.sugar = new Sugar();
            this.ice = new Ice();
        }

        public void SelectItem(string no)
        {
            if (no == "1")
                this.selectedItem = espresso;
            if (no == "2")
                this.selectedItem = water;
            if (no == "3")
                this.selectedItem = milk;
            if (no == "4")
                this.selectedItem = sugar;
            if (no == "5")
                this.selectedItem = ice;
        }

        private void UpdatePrice(string input)
        {
            try
            {
                var price = int.Parse(input);
                this.selectedItem.Update(price);

                Console.WriteLine($"{selectedItem.Name} 가격이 {selectedItem.Price} 원으로 업데이트 되었습니다.");
            }
            catch (Exception)
            {
                Console.WriteLine($"잘못된 값이 입력되었습니다. : {input}"); 
            }
        }

        private void AddSales()
        {
            this.totalPrice += selectedItem.Price;
            this.totalItem += selectedItem.Name + " ";

            Console.WriteLine($"{selectedItem.Name} 이(가) 추가되었습니다.");
        }

        private void PrintSales()
        {
            Console.WriteLine($"선택한 커피 재료는 [{totalItem}]이고 가격은 {totalPrice}원 입니다.");

            this.totalPrice = 0;
            this.totalItem = string.Empty;
        }

        private string ShowMenu(string msg)
        {
            Console.Clear();
            Console.WriteLine(msg);
            return Console.ReadLine();
        }

        private string ShowAddtionalMenu(string msg)
        {
            Console.Write(msg);
            return Console.ReadLine();
        }

        public void MainMenuLoop()
        {
            const string QuitMenu = "exit";

            while (true)
            {
                var selected = ShowMenu("주 메뉴\n1. 커피 판매, 2. 재료 가격 설정, exit. 종료");
                if (selected == QuitMenu)
                    break;

                SubMenuLoop(selected);
            }
        }

        private void SubMenuLoop(string subMenu)
        {
            const string QuitMenu = "Q";
            const string SalesMenu = "1";
            const string SettingMenu = "2";

            while (true)
            {
                var selected = ShowMenu("부 메뉴\n1. 에스프레소 커피, 2. 물, 3. 우유, 4. 설탕/시럽, 5. 얼음, Q. 주메뉴");
                if (selected == QuitMenu)
                    break;

                SelectItem(selected);
                if (subMenu == SalesMenu)
                {
                    if (selected == "") PrintSales();
                    else AddSales();
                }

                if (subMenu == SettingMenu)
                {
                    UpdatePrice(ShowAddtionalMenu($"현재 가격은 {this.selectedItem.Price}원 입니다. 새로운 가격을 입력하세요. : "));
                }

                Console.ReadLine();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var coffeeMachine = new CoffeeMachine();
            coffeeMachine.MainMenuLoop();
        }
    }
}
