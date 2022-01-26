using System;

namespace SimpleCalculator.Type
{
    class DoubleMath : AbstractMath<double>
    {
        public override double Division(double a, double b)
        {
            try
            {
                return a / b;
            }
            catch (Exception)
            {
                Console.WriteLine("0으로 나눌 수 없습니다.");
                return 0.0;
            }
        }

        public override double Minus(double a, double b) => a - b;

        public override double Multiply(double a, double b) => a * b;

        public override double Plus(double a, double b) => a + b;
    }
}
