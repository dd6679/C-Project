using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerMenu
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("메뉴\n1. 재생\n2. 일시 정지\n3. 빠른 재생\n4. 정지\n");
            Console.Write("메뉴 입력 : ");
            string n = Console.ReadLine();

            switch (n)
            {
                case "1":
                    Console.WriteLine("재생 중입니다.\n");
                    break;
                case "2":
                    Console.WriteLine("일시 정지하였습니다.\n");
                    break;
                case "3":
                    Console.WriteLine("빠른 재생 중입니다.\n");
                    break;
                case "4":
                    Console.WriteLine("정지되었습니다.\n");
                    break;
                default:
                    Console.WriteLine("잘못 입력했습니다.\n");
                    break;
            }
        }
    }
}
