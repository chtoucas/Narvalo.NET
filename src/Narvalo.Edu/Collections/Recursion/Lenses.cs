// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Collections.Recursion
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Fx;

    public static class Lenses
    {
        public static IEnumerable<TResult> Ana<TSource, TResult>(
            Func<TSource, TSource> iter,
            TSource seed,
            Func<TSource, TResult> resultSelector)
        {
            return Ana(iter, seed, resultSelector, _ => true);
        }

        public static IEnumerable<TResult> Ana<TSource, TResult>(
            Func<TSource, TSource> iter,
            TSource seed,
            Func<TSource, TResult> resultSelector,
            Func<TSource, bool> predicate)
        {
            TSource next = seed;

            while (predicate.Invoke(next)) {
                yield return resultSelector.Invoke(next);

                next = iter.Invoke(next);
            }
        }

        #region Generation Operators

        public static IEnumerable<int> Range(int start, int count)
        {
            return Ana(i => i + 1, 0, i => start + i, i => i < count);
        }

        public static IEnumerable<T> Repeat<T>(T value)
        {
            return Ana(i => i + 1, 0, i => value);
        }

        public static IEnumerable<T> Repeat<T>(T value, int count)
        {
            return Ana(i => i + 1, 0, i => value, i => i < count);
        }

        public static IEnumerable<T> Empty<T>()
        {
            return Ana(i => i, 0, i => default(T), i => false);
        }

        #endregion
    }
}
