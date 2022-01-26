using System;

namespace DotPixelEditor
{
    class Triangle : Figure
    {
        //public Triangle(/*int x*/) : base(/*x*/)
        //{

        //}

        public override void Draw()
        {
            base.Draw();

            for (int i = 0; i < num; i++)
            {
                SpaceX();
                for (int j = num - 1; j > i; j--)
                    Console.Write(' ');
                for (int j = 0; j < i + 1; j++)
                    Console.Write('▲');
                Console.WriteLine();
            }
        }
    }
}
