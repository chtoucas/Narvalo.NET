namespace Narvalo.Linq
{
    using System;
    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods for <see cref="System.Nullable&lt;T&gt;"/> in order to support Linq.
    /// </summary>
    public static partial class NullableExtensions
    {
        //// Restriction Operators

        public static TSource? Where<TSource>(
            this TSource? @this,
            Func<TSource, bool> predicate)
            where TSource : struct
        {
            Require.NotNull(predicate, "predicate");

            return @this.Filter(predicate);
        }

        //// Projection Operators

        public static TResult? Select<TSource, TResult>(
            this TSource? @this,
            Func<TSource, TResult> selector)
            where TSource : struct
            where TResult : struct
        {
            return @this.Map(selector);
        }

        public static TResult? SelectMany<TSource, TResult>(
            this TSource? @this,
            Func<TSource, TResult?> selector)
            where TSource : struct
            where TResult : struct
        {
            return @this.Bind(selector);
        }

        public static TResult? SelectMany<TSource, TMiddle, TResult>(
            this TSource? @this,
            Func<TSource, TMiddle?> valueSelector,
            Func<TSource, TMiddle, TResult> resultSelector)
            where TSource : struct
            where TMiddle : struct
            where TResult : struct
        {
            Require.NotNull(valueSelector, "valueSelector");
            Require.NotNull(resultSelector, "resultSelector");

            return @this.Bind(_ => valueSelector(_).Map(m => resultSelector(_, m)));
        }
    }
}
