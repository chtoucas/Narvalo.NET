namespace Narvalo.Linq
{
    using System;
    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods for <see cref="System.Nullable{T}"/> in order to support Linq.
    /// </summary>
    public static partial class NullableExtensions
    {
        //// Select

        public static Nullable<TResult> Select<TSource, TResult>(
            this Nullable<TSource> @this,
            Func<TSource, TResult> selector)
            where TSource : struct
            where TResult : struct
        {
            return @this.Map(selector);
        }

        //// SelectMany

        public static Nullable<TResult> SelectMany<TSource, TResult>(
            this Nullable<TSource> @this,
            Func<TSource, Nullable<TResult>> selector)
            where TSource : struct
            where TResult : struct
        {
            return @this.Bind(selector);
        }

        public static Nullable<TResult> SelectMany<TSource, TMiddle, TResult>(
            this Nullable<TSource> @this,
            Func<TSource, Nullable<TMiddle>> valueSelector,
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
