namespace Narvalo.Linq
{
    using System;
    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods for <see cref="Narvalo.Fx.Maybe&lt;T&gt;"/> in order to support Linq.
    /// </summary>
    public static class MaybeExtensions
    {
        //// Restriction Operators

        public static Maybe<TSource> Where<TSource>(
            this Maybe<TSource> @this,
            Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");

            return @this.Filter(predicate);
        }

        //// Projection Operators

        public static Maybe<TResult> Select<TSource, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, TResult> selector)
        {
            Require.Object(@this);

            return @this.Map(selector);
        }

        public static Maybe<TResult> SelectMany<TSource, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, Maybe<TResult>> selector)
        {
            return @this.Bind(selector);
        }

        public static Maybe<TResult> SelectMany<TSource, TMiddle, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, Maybe<TMiddle>> valueSelector,
            Func<TSource, TMiddle, TResult> resultSelector)
        {
            Require.NotNull(valueSelector, "valueSelector");
            Require.NotNull(resultSelector, "resultSelector");

            return @this.Bind(_ => valueSelector(_).Map(m => resultSelector(_, m)));
        }
    }
}
