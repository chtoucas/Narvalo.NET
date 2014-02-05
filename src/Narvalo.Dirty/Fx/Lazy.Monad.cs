namespace Narvalo.Fx
{
    using System;

    public static partial class Lazy
    {
        //internal static Lazy<T> η<T>(T value)
        //{
        //    return new Lazy<T>(() => value);
        //}

        internal static Lazy<T> μ<T>(Lazy<Lazy<T>> square)
        {
            return square.Value;
        }
    }
}
