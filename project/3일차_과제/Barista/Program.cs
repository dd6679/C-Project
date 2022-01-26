using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    class Program
    {
        // 재료 이름, 가격 총합
        static string global_menu = "";
        static int global_price = 0;

        // 주메뉴
        static string MainMenu()
        {
            Console.Clear();
            Console.WriteLine("주 메뉴\n1. 커피 판매, 2. 재료 가격 설정, exit. 종료");
            string n1 = Console.ReadLine();
            return n1;
        }

        // 부메뉴
        static void SubMenu()
        {
            Console.Clear();
            Console.WriteLine("부 메뉴\n1. 에스프레소 커피, 2. 물, 3. 우유, 4. 설탕/시럽, 5. 얼음, Q. 주메뉴");
        }

        // 커피 판매
        static void CoffeSales(ref Menu m)
        {
            global_menu += m.name + " ";
            global_price += m.price;
        }

        static void Main(string[] args)
        {
            Menu e = new Espresso();
            Menu w = new Water();
            Menu m = new Milk();
            Menu s = new Sugar();
            Menu i = new Ice();

            string n1 = "", n2 = "";

            while (n1 != "exit")
            {
                n1 = MainMenu();

                if (n1 == "1")
                {
                    SubMenu();
                    int enterCnt = 0;
                    
                    while (true)
                    {
                        n2 = Console.ReadLine();

                        switch (n2)
                        {
                            case "1":
                                CoffeSales(ref e);
                                break;
                            case "2":
                                CoffeSales(ref w);
                                break;
                            case "3":
                                CoffeSales(ref m);
                                break;
                            case "4":
                                CoffeSales(ref s);
                                break;
                            case "5":
                                CoffeSales(ref i);
                                break;
                            case "":
                                enterCnt++;
                                break;
                        }

                        if (enterCnt == 2)
                        {
                            Console.WriteLine($"선택한 커피 재료는 {global_menu}이고 가격은 {global_price}원 입니다.");
                            global_menu = "";
                            global_price = 0;
                            enterCnt = 0;
                            continue;
                        }

                        if (n2 == "Q")
                            break;
                    }
                }
                else if (n1 == "2")
                {
                    SubMenu();
                    while (true)
                    {
                        n2 = Console.ReadLine();
                        if (n2 == "Q")
                            break;

                        Console.Write("가격 : ");
                        try
                        {
                            int price = int.Parse(Console.ReadLine());

                            switch (n2)
                            {
                                case "1":
                                    e.Update(price);
                                    break;
                                case "2":
                                    w.Update(price);
                                    break;
                                case "3":
                                    m.Update(price);
                                    break;
                                case "4":
                                    s.Update(price);
                                    break;
                                case "5":
                                    i.Update(price);
                                    break;
                                default:
                                    break;
                            }
                        
                            Console.WriteLine("가격이 업데이트 되었습니다.");
                        }
                        catch (FormatException fe)
                        {
                            continue;
                        }
                    }

                }
            }
        }
    }
}
