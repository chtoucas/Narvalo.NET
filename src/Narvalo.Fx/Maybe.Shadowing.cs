// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;

    public partial struct Maybe<T>
    {
        #region Basic Monad functions

        public Maybe<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return IsSome ? Maybe.Of(selector.Invoke(Value)) : Maybe<TResult>.None;
        }

        public Maybe<TResult> Then<TResult>(Maybe<TResult> other)
            => IsSome ? other : Maybe<TResult>.None;

        #endregion

        #region Conditional execution of monadic expressions (Prelude)

        public void When(Func<T, bool> predicate, Action<T> action)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));

            if (IsSome && predicate.Invoke(Value)) { action.Invoke(Value); }
        }

        public void Unless(Func<T, bool> predicate, Action<T> action)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));

            if (IsSome && !predicate.Invoke(Value)) { action.Invoke(Value); }
        }

        #endregion

        #region Monadic lifting operators

        public Maybe<TResult> Zip<TSecond, TResult>(
            Maybe<TSecond> second,
            Func<T, TSecond, TResult> resultSelector)
        {
            Require.NotNull(resultSelector, nameof(resultSelector));

            return IsSome && second.IsSome
                ? Maybe.Of(resultSelector.Invoke(Value, second.Value))
                : Maybe<TResult>.None;
        }

        #endregion

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

        #region Non-standard extension methods.

        public Maybe<TResult> Coalesce<TResult>(
            Func<T, bool> predicate,
            Maybe<TResult> then,
            Maybe<TResult> otherwise)
        {
            Require.NotNull(predicate, nameof(predicate));

            return IsSome && predicate.Invoke(Value) ? then : otherwise;
        }

        public Maybe<TResult> Then<TResult>(Func<T, bool> predicate, Maybe<TResult> other)
        {
            Require.NotNull(predicate, nameof(predicate));

            return IsSome && predicate.Invoke(Value) ? other : Maybe<TResult>.None;
        }

        public Maybe<TResult> Otherwise<TResult>(Func<T, bool> predicate, Maybe<TResult> other)
        {
            Require.NotNull(predicate, nameof(predicate));

            return IsSome && !predicate.Invoke(Value) ? other : Maybe<TResult>.None;
        }

        public void Do(Action<T> action)
        {
            Require.NotNull(action, nameof(action));

            if (IsSome) { action.Invoke(Value); }
        }

        #endregion
    }

    public static partial class Sequence
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

    public static partial class Operators
    {
        internal static Maybe<IEnumerable<TSource>> FilterImpl<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<bool>> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));
            Warrant.NotNull<IEnumerable<TSource>>();

            return Maybe.Of(WhereAnyIterator(@this, predicate));
        }
    }
}