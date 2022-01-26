using System;

namespace SimpleCalculator.Type
{
    class IntMath : AbstractMath<int>
    {
        public override int Division(int a, int b)
        {
            try
            {
                return a / b;
            }
            catch (Exception)
            {
                Console.WriteLine("0으로 나눌 수 없습니다.");
                return 0;
            }
        }

        public override int Minus(int a, int b) => a - b;

        public override int Multiply(int a, int b) => a * b;

        public override int Plus(int a, int b) => a + b;
    }
}
