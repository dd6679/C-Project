using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingFogire
{
    class Program
    {
        static void Main(string[] args)
        {
            int width = 0, height = 0;

            Console.Write("폭 : ");
            width = int.Parse(Console.ReadLine());

            Console.Write("높이 : ");
            height = int.Parse(Console.ReadLine());

            for (int j = 0; j < height; j++)
            {
                for (int i = 1; i < width; i++)
                {
                    Console.Write('*');
                }
                Console.WriteLine('*');
            }
        }
    }
}
