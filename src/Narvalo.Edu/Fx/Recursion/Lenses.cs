// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Fx.Recursion
{
    using System;
    using System.Collections.Generic;

    public static class Lenses
    {
        public static IEnumerable<TResult> Unfold<TSource, TResult>(
            Func<TSource, Tuple<TResult, TSource>> generator,
            TSource seed)
        {
            TSource next = seed;

            while (true) {
                var res = generator.Invoke(next);

                if (res == null) {
                    yield break;
                }

                yield return res.Item1;

                next = res.Item2;
            }
        }

        public static IEnumerable<TResult> Ana<TSource, TResult>(
            Func<TSource, TSource> succ,
            TSource seed,
            Func<TSource, TResult> resultSelector)
        {
            return Ana(succ, seed, _ => true, resultSelector);
        }

        public static IEnumerable<TResult> Ana<TSource, TResult>(
            Func<TSource, TSource> succ,
            TSource seed,
            Func<TSource, bool> predicate,
            Func<TSource, TResult> resultSelector)
        {
            TSource next = seed;

            while (predicate.Invoke(next)) {
                yield return resultSelector.Invoke(next);

                next = succ.Invoke(next);
            }
        }

        public static IEnumerable<TResult> Finite<TSource, TResult>(
            Func<TSource, TSource> succ,
            TSource seed,
            Func<TSource, bool> predicate,
            Func<TSource, TResult> resultSelector)
        {
            return Unfold(_ => predicate(_) ? Tuple.Create(resultSelector(_), succ(_)) : null, seed);
        }

        public static IEnumerable<TResult> Infinite<TSource, TResult>(
            Func<TSource, TSource> succ,
            TSource seed,
            Func<TSource, TResult> resultSelector)
        {
            return Unfold(_ => Tuple.Create(resultSelector(_), succ(_)), seed);
        }

        #region Generation Operators

        public static IEnumerable<int> Range(int start, int count)
        {
            return Ana(i => i + 1, 0, i => i < count, i => start + i);
        }

        public static IEnumerable<T> Repeat<T>(T value)
        {
            return Ana(i => i + 1, 0, i => true, i => value);
        }

        public static IEnumerable<T> Repeat<T>(T value, int count)
        {
            return Ana(i => i + 1, 0, i => i < count, i => value);
        }

        public static IEnumerable<T> Empty<T>()
        {
            return Ana(i => i, 0, i => false, i => default(T));
        }

        #endregion
    }
}
