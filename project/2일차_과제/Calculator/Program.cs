using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            double num1 = 0, num2 = 0;
            string oper = "";

            Console.Write("첫번째 숫자 : ");
            num1 = double.Parse(Console.ReadLine());

            Console.Write("사칙연산자 : ");
            oper = Console.ReadLine();

            Console.Write("두번째 숫자 : ");
            num2 = double.Parse(Console.ReadLine());

            Console.Write("result : ");

            switch (oper)
            {
                case "+":
                    Console.WriteLine(num1 + num2 + "\n");
                    break;
                case "-":
                    Console.WriteLine(num1 - num2 + "\n");
                    break;
                case "*":
                    Console.WriteLine(num1 * num2 + "\n");
                    break;
                case "/":
                    try
                    {
                        Console.WriteLine(num1 / num2 + "\n");
                    }
                    catch (DivideByZeroException)
                    {
                        Console.WriteLine("0으로 나눌 수 없습니다.\n");
                    }
                    break;
                default:
                    Console.WriteLine("사칙연산자가 아닙니다.\n");
                    break;
            }
            
        }
    }
}
