using System;

namespace DotPixelEditor
{
    class Editor
    {
        //public int x;
        //public int y;

        //private Figure tr;
        //private Figure sq;
        //private Figure cir;

        //private Figure item;

        public Editor()
        {
            //this.tr = new Triangle(x);
            //this.sq = new Square(x);
            //this.cir = new Circle(x);
        }

        public Point InputPoint()
        {
            Console.Write("좌표 입력(X, Y) : ");
            string xy = Console.ReadLine();
            //string[] str_xy = xy.Split(',');
            //x = int.Parse(str_xy[0]);
            //y = int.Parse(str_xy[1]);
            return Point.Parse(xy);

            //item.UpdateX(pt.X);

            //for (int i = 0; i < y; i++)
            //    Console.WriteLine(" ");

            //return x;
        }

        public Figure SelectItem(string n)
        {
            switch (n)
            {
                case "1":
                    //this.item = tr;
                    return new Triangle();
                case "2":
                    //this.item = sq;
                    return new Square();
                case "3":
                    //this.item = cir;
                    return new Circle();
                default:
                    break;
            }
            return null;
        }

        public void MenuLoop()
        {
            while (true)
            {
                //Console.Clear();
                //Console.WriteLine("1. 삼각형, 2. 사각형, 3. 동그라미, Q. 종료");
                //string n = Console.ReadLine();
                string n = ShowMenu("1. 삼각형, 2. 사각형, 3. 동그라미, Q. 종료");
                if (n == "Q")
                    break;

                var item = SelectItem(n);
                if (item != null)
                {
                    var pt = InputPoint();
                    item.UpdatePoint(pt);
                    item.Draw();
                }

                ShowPressEnter();
            }
        }


        private string ShowMenu(string msg)
        {
            Console.Clear();
            Console.WriteLine(msg);
            return Console.ReadLine();
        }

        private void ShowPressEnter()
        {
            Console.WriteLine("\n\n계속 하려면 엔터를 눌러주세요.");
            Console.ReadLine();
        }
    }
}
