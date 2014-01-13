namespace Narvalo.Fx
{
    using System;

    public static class LazyExtensions
    {
        public static Lazy<X> Bind<T, X>(this Lazy<T> @this, Func<T, Lazy<X>> kun)
        {
            return new Lazy<X>(() => kun(@this.Value).Value);
        }

        public static Lazy<X> Map<T, X>(this Lazy<T> @this, Func<T, X> fun)
        {
            return new Lazy<X>(() => fun(@this.Value));
        }
    }
}
