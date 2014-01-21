namespace Narvalo.Fx
{
    using System;

    public static partial class LazyExtensions
    {
        public static Lazy<X> Bind<T, X>(this Lazy<T> @this, Func<T, Lazy<X>> kun)
        {
            Require.Object(@this);
            Require.NotNull(kun, "kun");

            return new Lazy<X>(() => kun.Invoke(@this.Value).Value);
        }

        public static Lazy<X> Map<T, X>(this Lazy<T> @this, Func<T, X> selector)
        {
            Require.Object(@this);
            Require.NotNull(selector, "selector");

            return new Lazy<X>(() => selector.Invoke(@this.Value));
        }
    }
}
