namespace Narvalo.Fx.Internal
{
    using System;

    internal static class LazyMonad<T>
    {
        public static Lazy<X> Bind<X>(Lazy<T> m, Func<T, Lazy<X>> kun)
        {
            return new Lazy<X>(() => kun(m.Value).Value);
        }

        public static Lazy<X> Map<X>(Lazy<T> m, Func<T, X> fun)
        {
            return new Lazy<X>(() => fun(m.Value));
        }

        public static Lazy<T> η(T value)
        {
            return new Lazy<T>(() => value);
        }

        public static Lazy<T> μ(Lazy<Lazy<T>> square)
        {
            return new Lazy<T>(() => square.Value.Value);
        }
    }
}
