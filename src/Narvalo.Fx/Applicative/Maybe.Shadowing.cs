// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Collections.Generic;

    public partial struct Maybe<T>
    {
        public Maybe<TResult> ReplaceBy<TResult>(TResult value)
            => IsSome ? Maybe.Of(value) : Maybe<TResult>.None;

        public Maybe<TResult> ContinueWith<TResult>(Maybe<TResult> other)
            => IsSome ? other : Maybe<TResult>.None;

        public Maybe<TResult> Zip<TSecond, TResult>(
            Maybe<TSecond> second,
            Func<T, TSecond, TResult> zipper)
        {
            Require.NotNull(zipper, nameof(zipper));

            return IsSome && second.IsSome
                ? Maybe.Of(zipper(Value, second.Value))
                : Maybe<TResult>.None;
        }

        #region Query Expression Pattern.

        public Maybe<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return IsSome ? Maybe.Of(selector(Value)) : Maybe<TResult>.None;
        }

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

            var outerKey = outerKeySelector(Value);
            var innerKey = innerKeySelector(inner.Value);

            return (comparer ?? EqualityComparer<TKey>.Default).Equals(outerKey, innerKey)
                ? Maybe.Of(resultSelector(Value, inner.Value))
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

            var outerKey = outerKeySelector(Value);
            var innerKey = innerKeySelector(inner.Value);

            return (comparer ?? EqualityComparer<TKey>.Default).Equals(outerKey, innerKey)
                ? Maybe.Of(resultSelector(Value, inner))
                : Maybe<TResult>.None;
        }

        #endregion
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

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Applicative;

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