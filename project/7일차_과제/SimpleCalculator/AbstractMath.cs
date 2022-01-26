namespace SimpleCalculator
{
    abstract class AbstractMath<T>
    {
        public abstract T Plus(T a, T b);
        public abstract T Minus(T a, T b);
        public abstract T Multiply(T a, T b);
        public abstract T Division(T a, T b);
    }
}
