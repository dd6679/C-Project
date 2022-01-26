using System;

namespace DotPixelEditor
{
    class Square : Figure
    {
        //public Square(/*int x*/) : base(/*x*/)
        //{

        //}
        public override void Draw()
        {
            base.Draw();

            for (int i = 0; i < num; i++)
            {
                SpaceX();
                for (int j = 0; j < num; j++)
                    Console.Write('■');
                Console.WriteLine();
            }
        }
    }
}
