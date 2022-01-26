using System;

namespace Barista
{
    class Menu
    {
        public string name;
        public int price;

        public void Update(int price)
        {
            this.price = price;
        }
    }
    class Espresso : Menu
    {
        public Espresso()
        {
            this.name = "에스프레소 커피";
            this.price = 2000;
        }
    }

    class Water : Menu
    {
        public Water()
        {
            this.name = "물";
            this.price = 1000;
        }
    }

    class Milk : Menu
    {
        public Milk()
        {
            this.name = "우유";
            this.price = 1200;
        }
    }

    class Sugar : Menu
    {
        public Sugar()
        {
            this.name = "설탕/시럽";
            this.price = 500;
        }
    }

    class Ice : Menu
    {
        public Ice()
        {
            this.name = "얼음";
            this.price = 700;
        }
    }

    class CofficeMachine
    {
        private Menu menu, e, w, m, s, i;

        public string sum_menu = "";
        public int sum_price = 0;

        public CofficeMachine()
        {
            this.e = new Espresso();
            this.w = new Water();
            this.m = new Milk();
            this.s = new Sugar();
            this.i = new Ice();
        }

        public void SelectItem(string n)
        {
            switch (n)
            {
                case "1":
                    this.menu = e;
                    break;
                case "2":
                    this.menu = w;
                    break;
                case "3":
                    this.menu = m;
                    break;
                case "4":
                    this.menu = s;
                    break;
                case "5":
                    this.menu = i;
                    break;
            }
        }
        public string ShowMenu(string s)
        {
            Console.Clear();
            Console.WriteLine(s);
            string n = Console.ReadLine();
            return n;
        }

        public void ShowSum()
        {
            Console.WriteLine($"선택한 커피 재료는 {sum_menu}이고 가격은 {sum_price}원 입니다.");
            sum_menu = "";
            sum_price = 0;
            Console.ReadLine();
        }

        public void SubLoop(string main_num)
        {
            while (true)
            {
                string sub_num = ShowMenu("부 메뉴\n1. 에스프레소 커피, 2. 물, 3. 우유, 4. 설탕/시럽, 5. 얼음, Q. 주메뉴");
                SelectItem(sub_num);

                if (sub_num == "Q")
                    break;

                if (main_num == "1")
                {
                    if (sub_num == "")
                    {
                        ShowSum();
                        continue;
                    }
                    else
                    {
                        sum_menu += this.menu.name + " ";
                        sum_price += this.menu.price;
                    }
                }
                else if (main_num == "2")
                {
                    Console.Write("가격 : ");

                    try
                    {
                        int price = int.Parse(Console.ReadLine());

                        this.menu.price = price;

                        Console.WriteLine("가격이 업데이트 되었습니다.");
                        Console.ReadLine();
                    }
                    catch (FormatException fe)
                    {
                        continue;
                    }
                }
            }
        }

        public void CoffeSales()
        {
            while (true)
            {
                string main_num = ShowMenu("주 메뉴\n1. 커피 판매, 2. 재료 가격 설정, exit. 종료");

                if (main_num == "exit")
                    break;

                sum_menu = "";
                sum_price = 0;

                SubLoop(main_num);
            }
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            CofficeMachine cm = new CofficeMachine();
            cm.CoffeSales();
        }
    }
}

