namespace Narvalo.Fx
{
    using System;

    public static partial class NullableExtensions
    {
        //// Match

        public static TResult Match<T, TResult>(this T? @this, Func<T, TResult> selector, TResult defaultValue)
            where T : struct
            where TResult : struct
        {
            return @this.Map(selector) ?? defaultValue;
        }

        public static TResult Match<T, TResult>(this T? @this, Func<T, TResult> selector, Func<TResult> defaultValueFactory)
            where T : struct
            where TResult : struct
        {
            Require.NotNull(defaultValueFactory, "defaultValueFactory");

            return @this.Match(selector, defaultValueFactory.Invoke());
        }

        //// Then

        public static TResult? Then<T, TResult>(this T? @this, TResult? other)
            where T : struct
            where TResult : struct
        {
            return @this.Bind(_ => other);
        }

        //// Filter

        public static T? Filter<T>(this T? @this, Predicate<T> predicate)
            where T : struct
        {
            return @this.Map(_ => predicate.Invoke(_)).Then(@this);
        }
    }
}
