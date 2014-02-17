// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Linq
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Edu.Fx;

    public static class Bananas
    {
        public static TResult Cata<T, TResult>(
            IEnumerable<T> source,
            TResult seed,
            Func<TResult, T, TResult> accumulator)
        {
            return Cata(source, seed, _ => true, accumulator);
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
            Kunc<TResult, T, TResult> accumulatorM)
        {
            return CataM(source, seed, _ => true, accumulatorM);
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
    }
}
