// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Collections.Recursion
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Narvalo.Edu.Samples;

    public static class Bananas
    {
        public static TResult Cata<TSource, TResult>(
            this IEnumerable<TSource> @this,
            TResult seed,
            Func<TResult, TSource, TResult> accumulator,
            Func<TResult, bool> predicate)
        {
            TResult result = seed;

            using (var iter = @this.GetEnumerator()) {
                while (predicate.Invoke(result) && iter.MoveNext()) {
                    result = accumulator.Invoke(result, iter.Current);
                }
            }

            return result;
        }

        public static Monad<TResult> CataM<TSource, TResult>(
            this IEnumerable<TSource> @this,
            TResult seed,
            Func<TResult, TSource, Monad<TResult>> accumulatorM,
            Func<Monad<TResult>, bool> predicate)
        {
            Monad<TResult> result = Monad.Return(seed);

            using (var iter = @this.GetEnumerator()) {
                while (predicate.Invoke(result) && iter.MoveNext()) {
                    result = result.Bind(_ => accumulatorM.Invoke(_, iter.Current));
                }
            }

            return result;
        }

        #region Quantifiers

        public static bool Any<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
        {
            return Cata(@this, true, (acc, item) => acc || predicate.Invoke(item), acc => !acc);
        }

        public static bool All<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
        {
            return Cata(@this, true, (acc, item) => acc && predicate.Invoke(item), acc => acc);
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
            return Cata(@this, seed, accumulator, _ => true);
        }

        public static Monad<TResult> AggregateM<T, TResult>(
            this IEnumerable<T> @this,
            TResult seed,
            Func<TResult, T, Monad<TResult>> accumulatorM)
        {
            return CataM(@this, seed, accumulatorM, _ => true);
        }

        #endregion
    }
}
