// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Linq
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Edu.Fx;

    // Exploring http://blogs.bartdesmet.net/blogs/bart/archive/2010/01/01/the-essence-of-linq-minlinq.aspx

    /*!
     * Catamorphism (Bananas)
     * ----------------------
     * M<T> -> ... -> TResult
     * M<T> -> TResult -> (TResult -> bool) -> (TResult -> T -> TResult) -> TResult
     * 
     * Bananas (|...|)
     * 
     * Aggregate, Fold, Count, All, Any, Contains, Sum, Max, Min, Average,...
     */

    static class Bananas
    {
        public static TResult Cata<T, TResult>(
            Func<T> iter,
            TResult seed,
            Func<TResult, T, TResult> accumulator)
        {
            return Cata(iter, seed, _ => true, accumulator);
        }

        public static TResult Cata<T, TResult>(
            Func<T> iter,
            TResult seed,
            Func<TResult, bool> predicate,
            Func<TResult, T, TResult> accumulator)
        {
            TResult result = seed;

            while (predicate.Invoke(result)) {
                result = accumulator.Invoke(result, iter.Invoke());
            }

            return result;
        }

        public static Monad<TResult> CataM<T, TResult>(
            Func<T> iter,
            TResult seed,
            Kunc<TResult, T, TResult> accumulatorM)
        {
            return CataM(iter, seed, _ => true, accumulatorM);
        }

        public static Monad<TResult> CataM<T, TResult>(
            Func<T> iter,
            TResult seed,
            Func<Monad<TResult>, bool> predicate,
            Kunc<TResult, T, TResult> accumulatorM)
        {
            Monad<TResult> result = Monad.Return(seed);

            while (predicate.Invoke(result)) {
                result = result.Bind(_ => accumulatorM.Invoke(_, iter.Invoke()));
            }

            return result;
        }

        #region IEnumerable

        public static TResult Cata<T, TResult>(
            this IEnumerable<T> @this,
            TResult seed,
            Func<TResult, T, TResult> accumulator)
        {
            return Cata(@this, seed, _ => true, accumulator);
        }

        public static TResult Cata<T, TResult>(
            this IEnumerable<T> @this,
            TResult seed,
            Func<TResult, bool> predicate,
            Func<TResult, T, TResult> accumulator)
        {
            TResult result = seed;

            using (var iter = @this.GetEnumerator()) {
                while (predicate.Invoke(result) && iter.MoveNext()) {
                    result = accumulator.Invoke(result, iter.Current);
                }
            }

            return result;
        }

        public static Monad<TResult> CataM<T, TResult>(
           this IEnumerable<T> @this,
            TResult seed,
            Kunc<TResult, T, TResult> accumulatorM)
        {
            return CataM(@this, seed, _ => true, accumulatorM);
        }

        public static Monad<TResult> CataM<T, TResult>(
           this IEnumerable<T> @this,
            TResult seed,
            Func<Monad<TResult>, bool> predicate,
            Kunc<TResult, T, TResult> accumulatorM)
        {
            Monad<TResult> result = Monad.Return(seed);

            using (var iter = @this.GetEnumerator()) {
                while (predicate.Invoke(result) && iter.MoveNext()) {
                    result = result.Bind(_ => accumulatorM.Invoke(_, iter.Current));
                }
            }

            return result;
        }

        #endregion

        #region Quantifiers

        public static bool Any<T>(Func<T> iter, Func<T, bool> predicate)
        {
            return Cata(iter, seed: true, predicate: acc => !acc, accumulator: (acc, item) => acc || predicate.Invoke(item));
        }

        public static bool All<T>(Func<T> iter, Func<T, bool> predicate)
        {
            return Cata(iter, seed: true, predicate: acc => acc, accumulator: (acc, item) => acc && predicate.Invoke(item));
        }

        public static bool Contains<T>(Func<T> iter, T value)
        {
            return Contains(iter, value, EqualityComparer<T>.Default);
        }

        public static bool Contains<T>(Func<T> iter, T value, IEqualityComparer<T> comparer)
        {
            return Any(iter, _ => comparer.Equals(_, value));
        }

        #endregion

        #region Aggregate Operators

        public static int Count<T>(Func<T> iter)
        {
            return Cata(iter, seed: 0, accumulator: (acc, item) => checked(acc + 1));
        }

        public static long LongCount<T>(Func<T> iter)
        {
            return Cata(iter, seed: 0L, accumulator: (acc, item) => checked(acc + 1));
        }

        public static int Sum(Func<int> iter)
        {
            return Sum(iter, _ => _);
        }

        public static int Sum<T>(Func<T> iter, Func<T, int> selector)
        {
            return Cata(iter, seed: 0, accumulator: (acc, item) => checked(acc + selector.Invoke(item)));
        }

        public static TResult Aggregate<T, TResult>(
            Func<T> iter,
            TResult seed,
            Func<TResult, T, TResult> accumulator)
        {
            return Cata(iter, seed, accumulator);
        }

        #endregion
    }
}
