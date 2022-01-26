using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator
{
    class Program
    {
        static char[] operators = { '+', '-', '*', '/' };
        static char oper;
        static string[] num;

        // 입출력 관리
        static public string ShowMenu(string s)
        {
            Console.Clear();
            Console.WriteLine(s);
            return Console.ReadLine();
        }

        // 문자열에서 숫자와 연산자 분리
        static public void Format(string s)
        {
            s = s.Replace(" ", String.Empty).Replace("=", String.Empty);

            foreach (char i in operators)
                if (s.Contains(i))
                    oper = i;

            num = s.Split(oper);
        }

        static void Main(string[] args)
        {
            // 작업 시간 : 3시간 반
            while (true)
            {
                string form = ShowMenu("계산식을 입력하세요.");

                Calculator<double> dcalc = new Calculator<double>();
                Calculator<int> icalc = new Calculator<int>();

                Format(form);

                if (form.Substring(form.Length - 1, 1) != "=" || num.Length != 2)
                    continue;

                // 점에 따라 타입 분기
                if (form.Contains('.'))
                    dcalc.Calculate(double.Parse(num[0]), double.Parse(num[1]), oper);
                else
                    icalc.Calculate(int.Parse(num[0]), int.Parse(num[1]), oper);

                Console.ReadLine();
            }

        }
    }
}
