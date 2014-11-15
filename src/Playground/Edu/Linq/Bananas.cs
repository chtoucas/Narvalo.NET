// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground.Edu.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Narvalo.Collections;

    // Linq from scratch.
    public static class Bananas
    {
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

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "long")]
        public static long LongCount<T>(this IEnumerable<T> @this)
        {
            return Aggregate(@this, 0L, (acc, item) => checked(acc + 1));
        }

        public static int Sum(this IEnumerable<int> @this)
        {
            return Sum(@this, i => i);
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
            return @this.Fold(seed, accumulator, _ => true);
        }

        #endregion
    }
}
