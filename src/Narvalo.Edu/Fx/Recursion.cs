// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Fx
{
    using System;
    using System.Collections.Generic;

    // Exploring http://blogs.bartdesmet.net/blogs/bart/archive/2010/01/01/the-essence-of-linq-minlinq.aspx
    static partial class Recursion
    {
        /* Catamorphism
         * ------------
         * M<T> -> ... -> TResult
         * M<T> -> TResult -> (TResult -> bool) -> (TResult -> T -> TResult) -> TResult
         * 
         * Bananas (|...|)
         */

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

        public static TResult Cata<T, TResult>(
            IEnumerable<T> source,
            TResult seed,
            Func<TResult, bool> predicate,
            Func<TResult, T, TResult> accumulator)
        {
            TResult result = seed;

            using (var iter = source.GetEnumerator()) {
                while (predicate.Invoke(result) && iter.MoveNext()) {
                    result = accumulator.Invoke(result, iter.Current);
                }
            }

            return result;
        }

        public static Monad<TResult> CataM<T, TResult>(
            IEnumerable<T> source,
            TResult seed,
            Func<Monad<TResult>, bool> predicate,
            Kunc<TResult, T, TResult> accumulatorM)
        {
            Monad<TResult> result = Monad.Return(seed);

            using (var iter = source.GetEnumerator()) {
                while (predicate.Invoke(result) && iter.MoveNext()) {
                    result = result.Bind(_ => accumulatorM.Invoke(_, iter.Current));
                }
            }

            return result;
        }

        /* Anamorphism
         * -----------
         * T -> ... -> M<TResult>
         * (T -> T) -> T -> (T -> bool) -> (T --> TResult) -> M<TResult>
         * 
         * Lenses [(...)]
         */

        public static IEnumerable<TResult> Ana<T, TResult>(
            Func<T, T> iter,
            T seed,
            Func<T, bool> predicate,
            Func<T, TResult> selector)
        {
            for (T t = seed; predicate.Invoke(t); t = iter.Invoke(t)) {
                yield return selector.Invoke(t);
            }
        }

        public static IEnumerable<TResult> Ana<T, TResult>(
            IEnumerable<T> source,
            T seed,
            Func<T, bool> predicate,
            Func<T, TResult> selector)
        {
            using (var iter = source.GetEnumerator()) {
                T value = seed;

                while (predicate.Invoke(value)) {
                    yield return selector.Invoke(value);

                    if (iter.MoveNext()) {
                        value = iter.Current;
                    }
                    else {
                        yield break;
                    }
                }
            }
        }

        /* Bind
         * ----
         * M<T> -> (T -> bool) -> (T -> M<C>) -> (T -> C -> TResult) -> M<TResult>
         * M<TResult> Bind<TResult>(Func<T, M<TResult>> fun)
         */

        public static Monad<TResult> Bind<T, TInner, TResult>(
            Monad<T> source,
            Func<T, bool> predicate,
            Func<T, Monad<TInner>> selector,
            Func<T, TInner, TResult> accumulator)
        {
            throw new NotImplementedException();
        }
    }
}
