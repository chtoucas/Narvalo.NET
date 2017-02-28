// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;

    public partial struct Maybe<T>
    {
        public Maybe<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return IsSome ? Maybe.Of(selector.Invoke(Value)) : Maybe<TResult>.None;
        }

        public Maybe<TResult> ReplaceBy<TResult>(TResult value)
            => IsSome ? Maybe.Of(value) : Maybe<TResult>.None;

        public Maybe<TResult> Then<TResult>(Maybe<TResult> other)
            => IsSome ? other : Maybe<TResult>.None;

        public Maybe<TResult> Zip<TSecond, TResult>(
            Maybe<TSecond> second,
            Func<T, TSecond, TResult> resultSelector)
        {
            Require.NotNull(resultSelector, nameof(resultSelector));

            return IsSome && second.IsSome
                ? Maybe.Of(resultSelector.Invoke(Value, second.Value))
                : Maybe<TResult>.None;
        }

        #region LINQ extensions

        public Maybe<TResult> Join<TInner, TKey, TResult>(
            Maybe<TInner> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            Require.NotNull(outerKeySelector, nameof(outerKeySelector));
            Require.NotNull(innerKeySelector, nameof(innerKeySelector));
            Require.NotNull(resultSelector, nameof(resultSelector));

            if (IsNone || inner.IsNone) { return Maybe<TResult>.None; }

            var outerKey = outerKeySelector.Invoke(Value);
            var innerKey = innerKeySelector.Invoke(inner.Value);

            return (comparer ?? EqualityComparer<TKey>.Default).Equals(outerKey, innerKey)
                ? Maybe.Of(resultSelector.Invoke(Value, inner.Value))
                : Maybe<TResult>.None;
        }

        public Maybe<TResult> GroupJoin<TInner, TKey, TResult>(
            Maybe<TInner> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, Maybe<TInner>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            Require.NotNull(outerKeySelector, nameof(outerKeySelector));
            Require.NotNull(innerKeySelector, nameof(innerKeySelector));
            Require.NotNull(resultSelector, nameof(resultSelector));

            if (IsNone || inner.IsNone) { return Maybe<TResult>.None; }

            var outerKey = outerKeySelector.Invoke(Value);
            var innerKey = innerKeySelector.Invoke(inner.Value);

            return (comparer ?? EqualityComparer<TKey>.Default).Equals(outerKey, innerKey)
                ? Maybe.Of(resultSelector.Invoke(Value, inner))
                : Maybe<TResult>.None;
        }

        #endregion

        public Maybe<TResult> Coalesce<TResult>(
            Func<T, bool> predicate,
            Maybe<TResult> thenResult,
            Maybe<TResult> elseResult)
        {
            Require.NotNull(predicate, nameof(predicate));

            return IsSome && predicate.Invoke(Value) ? thenResult : elseResult;
        }

        public Maybe<TResult> If<TResult>(Func<T, bool> predicate, Maybe<TResult> thenResult)
        {
            Require.NotNull(predicate, nameof(predicate));

            return IsSome && predicate.Invoke(Value) ? thenResult : Maybe<TResult>.None;
        }
    }

    public static partial class Maybe
    {
        internal static Maybe<IEnumerable<TSource>> CollectImpl<TSource>(this IEnumerable<Maybe<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));

            return Maybe.Of(CollectAnyIterator(@this));
        }
    }
}

namespace Narvalo.Fx.Linq
{
    using System;
    using System.Collections.Generic;

    public static partial class Qperators
    {
        internal static Maybe<IEnumerable<TSource>> WhereByImpl<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<bool>> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));

            return Maybe.Of(WhereAnyIterator(@this, predicate));
        }
    }
}