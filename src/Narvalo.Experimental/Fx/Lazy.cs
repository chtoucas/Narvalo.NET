namespace Narvalo.Fx
{
    using System;
    using Narvalo.Fx.Internal;

    public static partial class Lazy
    {
        #region + Create +

        public static Lazy<T> Create<T>(T value)
        {
            return LazyMonad<T>.η(value);
        }

        #endregion

        #region + Join +

        public static Lazy<T> Join<T>(Lazy<Lazy<T>> option)
        {
            return LazyMonad<T>.μ(option);
        }

        #endregion

        #region + Compose +

        public static Lazy<TResult> Compose<TSource, TMiddle, TResult>(
            Func<TMiddle, Lazy<TResult>> kunB,
            Func<TSource, Lazy<TMiddle>> kunA,
            TSource value)
        {
            return kunA(value).Bind(kunB);
        }

        #endregion
    }
}
