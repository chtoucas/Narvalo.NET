// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Linq
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Applicative;
    using Narvalo.Linq;

    // LINQ from scratch.
    public static class Qperators
    {
        #region Anamorphisms aka Lenses |(...)|

        #region Generation

        public static IEnumerable<int> Range(int start, int count)
            => Sequence.Gather(0, i => i + 1, i => start + i, i => i < count);

        public static IEnumerable<T> Repeat<T>(T value)
            => Sequence.Gather(0, i => i + 1, i => value);

        public static IEnumerable<T> Repeat<T>(T value, int count)
            => Sequence.Gather(0, i => i + 1, i => value, i => i < count);

        public static IEnumerable<T> Empty<T>()
            => Sequence.Gather(0, Stubs<int>.Identity, Stubs<int, T>.AlwaysDefault, Stubs<int>.AlwaysFalse);

        #endregion

        #endregion

        #region Catamorphisms aka Bananas (|...|)

        #region Quantifiers

        public static bool Any<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
            => @this.Aggregate(true, (acc, item) => acc || predicate.Invoke(item), acc => !acc);

        public static bool All<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
            => @this.Aggregate(true, (acc, item) => acc && predicate.Invoke(item), acc => acc);

        public static bool Contains<T>(this IEnumerable<T> @this, T value)
            => Contains(@this, value, EqualityComparer<T>.Default);

        public static bool Contains<T>(this IEnumerable<T> @this, T value, IEqualityComparer<T> comparer)
            => Any(@this, _ => comparer.Equals(_, value));

        #endregion

        #region Aggregation

        public static int Count<T>(this IEnumerable<T> @this)
            => Aggregate(@this, 0, (acc, item) => checked(acc + 1));

        public static long LongCount<T>(this IEnumerable<T> @this)
            => Aggregate(@this, 0L, (acc, item) => checked(acc + 1));

        public static int Sum(this IEnumerable<int> @this)
            => Sum(@this, Stubs<int>.Identity);

        public static int Sum<T>(this IEnumerable<T> @this, Func<T, int> selector)
            => Aggregate(@this, 0, (acc, item) => checked(acc + selector.Invoke(item)));

        public static TResult Aggregate<T, TResult>(
            this IEnumerable<T> @this,
            TResult seed,
            Func<TResult, T, TResult> accumulator)
            => @this.Aggregate(seed, accumulator, Stubs<TResult>.AlwaysTrue);

        #endregion

        #endregion
    }
}