using System;

namespace DotPixelEditor
{
    class Figure
    {
        protected const int num = 4;
        //protected int x;
        protected Point _location;

        //public Figure(/*int x*/)
        //{
        //    //this.num =8;
        //    //this.x = x;
        //}
        public virtual void Draw()
        {
            for (int i = 0; i < _location.Y; i++)
                Console.WriteLine(" ");
        }
        //public void UpdateX(int x) => this.x = x;
        public void UpdatePoint(Point pt) => this._location = pt;

        public void SpaceX()
        {
            for (int k = 0; k < _location.X; k++)
                Console.Write(" ");
        }
    }
}
