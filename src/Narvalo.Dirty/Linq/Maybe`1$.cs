namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods for <see cref="Narvalo.Fx.Maybe&lt;T&gt;"/> in order to support Linq.
    /// </summary>
    public static class MaybeExtensions
    {
        //// Join Operators

        public static Maybe<TResult> Join<TSource, TInner, TKey, TResult>(
            this Maybe<TSource> @this,
            Maybe<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector)
        {
            return Join(@this, inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default);
        }

        public static Maybe<TResult> Join<TSource, TInner, TKey, TResult>(
            this Maybe<TSource> @this,
            Maybe<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            Require.Object(@this);
            Require.NotNull(inner, "inner");
            Require.NotNull(outerKeySelector, "valueSelector");
            Require.NotNull(innerKeySelector, "innerKeySelector");
            Require.NotNull(resultSelector, "resultSelector");

            if (@this.IsNone || inner.IsNone) {
                return Maybe<TResult>.None;
            }

            var outerKey = outerKeySelector.Invoke(@this.Value);
            var innerKey = innerKeySelector.Invoke(inner.Value);

            return (comparer ?? EqualityComparer<TKey>.Default).Equals(outerKey, innerKey)
                ? Maybe.Create(resultSelector.Invoke(@this.Value, inner.Value))
                : Maybe<TResult>.None;
        }
    }
}
