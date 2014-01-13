namespace Narvalo.Fx
{
    using System;

    public static class Lazy
    {
        public static Lazy<T> Create<T>(T value)
        {
            return η(value);
        }

        public static Lazy<T> Join<T>(Lazy<Lazy<T>> option)
        {
            return μ(option);
        }

        public static Lazy<TResult> Compose<TSource, TMiddle, TResult>(
            Func<TMiddle, Lazy<TResult>> kunB,
            Func<TSource, Lazy<TMiddle>> kunA,
            TSource value)
        {
            return kunA(value).Bind(kunB);
        }

        internal static Lazy<T> η<T>(T value)
        {
            return new Lazy<T>(() => value);
        }

        internal static Lazy<T> μ<T>(Lazy<Lazy<T>> square)
        {
            return new Lazy<T>(() => square.Value.Value);
        }
    }
}
