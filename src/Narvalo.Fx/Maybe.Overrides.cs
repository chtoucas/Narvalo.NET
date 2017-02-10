// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // Overrides for auto-generated (extension) methods.
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

            return predicate.Invoke(Value) ? then : otherwise;
        }

        public Maybe<TResult> Then<TResult>(Func<T, bool> predicate, Maybe<TResult> other)
        {
            Expect.NotNull(predicate);

            return predicate.Invoke(Value) ? other : Maybe<TResult>.None;
        }

        public Maybe<TResult> Otherwise<TResult>(Func<T, bool> predicate, Maybe<TResult> other)
        {
            Expect.NotNull(predicate);

            return predicate.Invoke(Value) ? Maybe<TResult>.None : other;
        }

        public void Apply(Action<T> action)
        {
            Require.NotNull(action, nameof(action));

            if (IsSome) { action.Invoke(Value); }
        }

        #endregion
    }

    // Overrides for auto-generated (extension) methods on IEnumerable<Maybe<T>>.
    public static partial class EnumerableExtensions
    {
        internal static Maybe<IEnumerable<TSource>> CollectCore<TSource>(this IEnumerable<Maybe<TSource>> @this)
        {
            Demand.NotNull(@this);

            var list = new List<TSource>();

            foreach (var m in @this)
            {
                if (m.IsNone) { continue; }

                list.Add(m.Value);
            }

            return Maybe.Of(list.AsEnumerable());
        }
    }
}

namespace Narvalo.Fx.More
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // Overrides for auto-generated (extension) methods on IEnumerable<T>.
    public static partial class EnumerableExtensions
    {
        internal static Maybe<IEnumerable<TSource>> FilterCore<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<bool>> predicateM)
        {
            Demand.NotNull(@this);
            Demand.NotNull(predicateM);
            Warrant.NotNull<IEnumerable<TSource>>();

            // See https://byorgey.wordpress.com/2007/06/26/deducing-code-from-types-filterm/
            // http://stackoverflow.com/questions/28872396/haskells-filterm-with-filterm-x-true-false-1-2-3
            // REVIEW: Recursion would be nicer, no?
            // or, is it equivalent to:
            // > var seq = @this.Where(_ => predicateM.Invoke(_).ValueOrElse(false));
            // > return Maybe.Of(seq);

            var list = new List<TSource>();

            foreach (var item in @this)
            {
                predicateM.Invoke(item).Apply(_ => { if (_) { list.Add(item); } });
            }

            return Maybe.Of(list.AsEnumerable());
        }
    }
}