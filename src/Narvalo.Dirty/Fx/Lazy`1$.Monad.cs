namespace Narvalo.Fx
{
    using System;

    public static partial class LazyExtensions
    {
        public static Lazy<TResult> Bind<TSource, TResult>(this Lazy<TSource> @this, Func<TSource, Lazy<TResult>> selector)
        {
            Require.Object(@this);
            Require.NotNull(selector, "selector");

            return new Lazy<TResult>(() => selector.Invoke(@this.Value).Value);
        }

        public static Lazy<TResult> Map<TSource, TResult>(this Lazy<TSource> @this, Func<TSource, TResult> selector)
        {
            Require.Object(@this);
            Require.NotNull(selector, "selector");

            return new Lazy<TResult>(() => selector.Invoke(@this.Value));
        }
    }
}
