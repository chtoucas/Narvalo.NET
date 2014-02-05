namespace Narvalo.Fx
{
    using System;

    public static partial class NullableExtensions
    {
        public static TResult? Bind<T, TResult>(this T? @this, Func<T, TResult?> selector)
            where T : struct
            where TResult : struct
        {
            Require.NotNull(selector, "selector");

            return @this.HasValue ? selector.Invoke(@this.Value) : null;
        }

        public static TResult? Map<T, TResult>(this T? @this, Func<T, TResult> selector)
            where T : struct
            where TResult : struct
        {
            Require.NotNull(selector, "selector");

            return @this.HasValue ? (TResult?)selector.Invoke(@this.Value) : null;
        }
    }
}
