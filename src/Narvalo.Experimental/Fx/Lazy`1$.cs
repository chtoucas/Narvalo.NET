namespace Narvalo.Fx
{
    using System;
    using Narvalo.Fx.Internal;

    public static class LazyExtensions
    {
        public static Lazy<X> Bind<T, X>(this Lazy<T> @this, Func<T, Lazy<X>> kun)
        {
            return LazyMonad<T>.Bind(@this, kun);
        }

        public static Lazy<X> Map<T, X>(this Lazy<T> @this, Func<T, X> fun)
        {
            return LazyMonad<T>.Map(@this, fun);
        }
    }
}
