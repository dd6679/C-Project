using System;

namespace DotPixelEditor
{
    class Figure
    {
        protected int num;
        protected int x;
        
        public Figure(int x)
        {
            this.num = 8;
            this.x = x;
        }
        public virtual void Draw()
        {
        }
        public void UpdateX(int x) => this.x = x;

        public void SpaceX()
        {
            for (int k = 0; k < x; k++)
                Console.Write("#");
        }
    }

    class Triangle : Figure
    {
        public Triangle(int x) : base(x)
        {

        }

        public override void Draw()
        {
            
            for (int i = 0; i < num; i++)
            {
                SpaceX();
                for (int j = num - 1; j > i; j--)
                    Console.Write(' ');
                for (int j = 0; j < i+1; j++)
                    Console.Write('▲');
                Console.WriteLine();
            }
        }
    }

    class Square : Figure
    {
        public Square(int x) : base(x)
        {

        }
        public override void Draw()
        {
            for (int i = 0; i < num; i++)
            {
                SpaceX();
                for (int j = 0; j < num; j++)
                    Console.Write('■');
                Console.WriteLine();
            }
        }
    }

    class Circle : Figure
    {
        public Circle(int x) : base(x)
        {

        }
        public override void Draw()
        {
            for (int i = -num + 1; i <= num; i += 2)
            {
                SpaceX();
                for (int j = -num; j <= num; j++)
                {
                    if ((i * i + j * j) <= num * num - num / 1.3 && (i * i + j * j) <= num * num + num / 1.3)
                        Console.Write('*');
                    else
                        Console.Write(' ');
                }
                Console.WriteLine();
            }
        }
    }

    class Editor
    {
        public int x;
        public int y;

        private Figure tr;
        private Figure sq;
        private Figure cir;

        private Figure item;

        public Editor()
        {
            this.tr = new Triangle(x);
            this.sq = new Square(x);
            this.cir = new Circle(x);
        }

        public int DivideXY()
        {
            Console.Write("좌표 입력(X, Y) : ");
            string xy = Console.ReadLine();
            string[] str_xy = xy.Split(", ");
            x = int.Parse(str_xy[0]);
            y = int.Parse(str_xy[1]);

            item.UpdateX(x);

            for (int i = 0; i < y; i++)
                Console.WriteLine("#");

            return x;
        }

        public void SelectItem(string n)
        {
            switch (n)
            {
                case "1":
                    this.item = tr;
                    break;
                case "2":
                    this.item = sq;
                    break;
                case "3":
                    this.item = cir;
                    break;
                default:
                    break;
            }
        }

        public void MenuLoop()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. 삼각형, 2. 사각형, 3. 동그라미, Q. 종료");
                string n = Console.ReadLine();
                if (n == "Q")
                    break;

                SelectItem(n);
                DivideXY();
                item.Draw();
                
                Console.ReadLine();
            }
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Editor e = new Editor();
            e.MenuLoop();
        }
    }
}
