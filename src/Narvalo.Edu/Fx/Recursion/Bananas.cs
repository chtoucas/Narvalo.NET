// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Fx.Recursion
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Edu.Fx;

    public static class Bananas
    {
        public static TResult Cata<TSource, TResult>(
            this IEnumerable<TSource> @this,
            TResult seed,
            Func<TResult, bool> predicate,
            Func<TResult, TSource, TResult> accumulator)
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
            Func<Monad<TResult>, bool> predicate,
            Kunc<TResult, TSource, TResult> accumulatorM)
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
            return @this.Cata(true, acc => !acc, (acc, item) => acc || predicate.Invoke(item));
        }

        public static bool All<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
        {
            return @this.Cata(true, acc => acc, (acc, item) => acc && predicate.Invoke(item));
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
            return @this.Cata(seed, _ => true, accumulator);
        }

        public static Monad<TResult> AggregateM<T, TResult>(
            this IEnumerable<T> @this,
            TResult seed,
            Kunc<TResult, T, TResult> accumulatorM)
        {
            return CataM(@this, seed, _ => true, accumulatorM);
        }

        #endregion
    }
}
