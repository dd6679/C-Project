using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleCalculator.Type;

namespace SimpleCalculator
{
    class Calculator <T>
    {
        T result;

        AbstractMath<T> math;

        public Calculator()
        {
            if (typeof(T) == typeof(double))
                math = new DoubleMath() as AbstractMath<T>;
            else if (typeof(T) == typeof(int))
                math = new IntMath() as AbstractMath<T>;
        }

        // 연산자에 따라 계산 후 답 출력
        public void Calculate(T a, T b, char oper)
        {
            switch (oper)
            {
                case '+':
                    result = math.Plus(a, b);
                    break;
                case '-':
                    result = math.Minus(a, b);
                    break;
                case '*':
                    result = math.Multiply(a, b);
                    break;
                case '/':
                    result = math.Division(a, b);
                    break;
                default:
                    break;
            }

            Console.Write($"답 : {result}");
        }
    }
}
