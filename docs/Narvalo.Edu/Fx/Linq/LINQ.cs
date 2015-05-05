// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Linq
{
    using System;
    using System.Collections.Generic;

    // LINQ from scratch.
    public static class LINQ
    {
        #region Anamorphisms aka Lenses

        #region Generation Operators

        public static IEnumerable<int> Range(int start, int count)
        {
            return Sequence.Gather(0, Sequence.Increment, i => start + i, i => i < count);
        }

        public static IEnumerable<T> Repeat<T>(T value)
        {
            return Sequence.Gather(0, Sequence.Increment, i => value);
        }

        public static IEnumerable<T> Repeat<T>(T value, int count)
        {
            return Sequence.Gather(0, Sequence.Increment, i => value, i => i < count);
        }

        public static IEnumerable<T> Empty<T>()
        {
            return Sequence.Gather(0, Sequence.Identity, Sequence<T>.AlwaysDefault, Sequence.AlwaysFalse);
        }

        #endregion

        #endregion

        #region Catamorphisms aka Bananas

        #region Quantifiers

        public static bool Any<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
        {
            return @this.Fold(true, (acc, item) => acc || predicate.Invoke(item), acc => !acc);
        }

        public static bool All<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
        {
            return @this.Fold(true, (acc, item) => acc && predicate.Invoke(item), acc => acc);
        }

        public static bool Contains<T>(this IEnumerable<T> @this, T value)
        {
            return Contains(@this, value, EqualityComparer<T>.Default);
        }

        public static bool Contains<T>(this IEnumerable<T> @this, T value, IEqualityComparer<T> comparer)
        {
            return Any(@this, _ => comparer.Equals(_, value));
        }

        #endregion

        #region Aggregate Operators

        public static int Count<T>(this IEnumerable<T> @this)
        {
            return Aggregate(@this, 0, (acc, item) => checked(acc + 1));
        }

        public static long LongCount<T>(this IEnumerable<T> @this)
        {
            return Aggregate(@this, 0L, (acc, item) => checked(acc + 1));
        }

        public static int Sum(this IEnumerable<int> @this)
        {
            return Sum(@this, Stubs<int>.Identity);
        }

        public static int Sum<T>(this IEnumerable<T> @this, Func<T, int> selector)
        {
            return Aggregate(@this, 0, (acc, item) => checked(acc + selector.Invoke(item)));
        }

        public static TResult Aggregate<T, TResult>(
            this IEnumerable<T> @this,
            TResult seed,
            Func<TResult, T, TResult> accumulator)
        {
            return @this.Fold(seed, accumulator, Stubs<TResult>.AlwaysTrue);
        }

        #endregion

        #endregion
    }
}
