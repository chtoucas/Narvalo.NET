// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Fx;

    /// <summary>
    /// Provides limited support for the Query Expression Pattern with <see cref="Narvalo.Fx.Maybe&lt;T&gt;"/>.
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

            return @this.Bind(_ => predicate.Invoke(_) ? @this : Maybe<TSource>.None);
        }

        //// Projection Operators

        public static Maybe<TResult> Select<TSource, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, TResult> selector)
        {
            Require.Object(@this);

            return @this.Map(selector);
        }

        public static Maybe<TResult> SelectMany<TSource, TMiddle, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, Maybe<TMiddle>> valueSelector,
            Func<TSource, TMiddle, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(valueSelector, "valueSelector");
            Require.NotNull(resultSelector, "resultSelector");

            return @this.Bind(_ => valueSelector(_).Map(m => resultSelector(_, m)));
        }

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
