// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Collections.Generic;

    public partial struct Maybe<T>
    {
        // If "applicative" contains "f" and "this" contains "x", return a Maybe of "f(x)".
        public Maybe<TResult> Gather<TResult>(Maybe<Func<T, TResult>> applicative)
            => IsSome && applicative.IsSome ? Maybe<TResult>.η(applicative.Value(Value)) : Maybe<TResult>.None;

        public Maybe<TResult> ReplaceBy<TResult>(TResult value)
            => IsSome ? Maybe<TResult>.η(value) : Maybe<TResult>.None;

        public Maybe<TResult> ContinueWith<TResult>(Maybe<TResult> other)
            => IsSome ? other : Maybe<TResult>.None;

        public Maybe<T> PassBy<TOther>(Maybe<TOther> other)
            // Returning "this" is not very "functional", but since Maybe is a value type, that's fine.
            => other.IsSome ? this : None;

        public Maybe<Unit> Skip()
            => IsSome ? Maybe.Unit : Maybe.None;

        public Maybe<TResult> Zip<TSecond, TResult>(Maybe<TSecond> second, Func<T, TSecond, TResult> zipper)
        {
            Require.NotNull(zipper, nameof(zipper));

            return IsSome && second.IsSome ? Maybe<TResult>.η(zipper(Value, second.Value)) : Maybe<TResult>.None;
        }

        #region Query Expression Pattern

        public Maybe<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return IsSome ? Maybe<TResult>.η(selector(Value)) : Maybe<TResult>.None;
        }

        public Maybe<T> Where(Func<T, bool> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));

            // Returning "this" is not very "functional",
            // but since Maybe is a value type, it is fine.
            return IsSome && predicate(Value) ? this : None;
        }

        public Maybe<TResult> SelectMany<TMiddle, TResult>(
            Func<T, Maybe<TMiddle>> selector,
            Func<T, TMiddle, TResult> resultSelector)
        {
            Require.NotNull(selector, nameof(selector));
            Require.NotNull(resultSelector, nameof(resultSelector));

            if (IsNone) { return Maybe<TResult>.None; }
            var middle = selector(Value);

            if (middle.IsNone) { return Maybe<TResult>.None; }
            return Maybe<TResult>.η(resultSelector(Value, middle.Value));
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
            Require.NotNull(comparer, nameof(comparer));

            if (IsSome && inner.IsSome)
            {
                var outerKey = outerKeySelector(Value);
                var innerKey = innerKeySelector(inner.Value);

                if (comparer.Equals(outerKey, innerKey))
                {
                    return Maybe<TResult>.η(resultSelector(Value, inner.Value));
                }
            }

            return Maybe<TResult>.None;
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
            Require.NotNull(comparer, nameof(comparer));

            if (IsSome && inner.IsSome)
            {
                var outerKey = outerKeySelector(Value);
                var innerKey = innerKeySelector(inner.Value);

                if (comparer.Equals(outerKey, innerKey))
                {
                    return Maybe<TResult>.η(resultSelector(Value, inner));
                }
            }

            return Maybe<TResult>.None;
        }

        #endregion
    }
}
