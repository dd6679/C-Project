using System;

namespace DotPixelEditor
{
    class Circle : Figure
    {
        //public Circle(/*int x*/) : base(/*x*/)
        //{

        //}
        public override void Draw()
        {
            base.Draw();

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
}
