using System;
using CreatingGameCharacter.Item;

namespace CreatingGameCharacter
{
    class Character
    {
        private AbstractItem[] items = new AbstractItem[3] { null, null, null };

        // 주메뉴 - 1.아이템 선택의 부메뉴 선택
        public void SelectItem(string n)
        {
            switch (n)
            {
                case "1":
                    this.items[0] = new Sword();
                    break;
                case "2":
                    this.items[1] = new Bow();
                    break;
                case "3":
                    this.items[2] = new MagicBook();
                    break;
                default:
                    break;
            }
        }
        private string ShowMenu(string msg)
        {
            Console.Clear();
            Console.WriteLine(msg);
            return Console.ReadLine();
        }

        // 주메뉴 - 2.캐릭터 보기의 부메뉴 선택
        public void DescribeCharacter(string n)
        {
            for (int i = 0; i < 3; i++)
            {
                if (this.items[i] == null)
                    continue;
                if (n == "1")
                    this.items[i].Introduce();
                if (n == "2")
                    this.items[i].attack();
                if (n == "3")
                    Console.WriteLine(this.items[i].name);
            }
        }

        // 주메뉴 루프
        public void MainMenuLoop()
        {
            while (true)
            {
                string selected = ShowMenu("1. 아이템 선택, 2. 캐릭터 보기, Q.종료");
                if (selected == "Q")
                    break;

                SubMenuLoop(selected);
            }
        }

        // 부메뉴 루프
        private void SubMenuLoop(string subMenu)
        {
            const string MainOne = "1";
            const string MainTwo = "2";
            string selected;

            while (true)
            {
                if (subMenu == MainOne)
                {
                    selected = ShowMenu("1. 도검, 2. 활, 3. 마법책, Q.상위메뉴");
                    SelectItem(selected);
                    if (selected == "Q")
                        break;
                }

                if (subMenu == MainTwo)
                {
                    selected = ShowMenu("1. 소개, 2. 공격, 3. 착용아이템 보기, Q.상위메뉴");
                    DescribeCharacter(selected);
                    if (selected == "Q")
                        break;
                }

                Console.ReadLine();
            }
        }
    }

}
