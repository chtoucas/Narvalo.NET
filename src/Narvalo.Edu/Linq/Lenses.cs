// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Linq
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Fx;

    // Linq from scratch.
    public static class Lenses
    {
        #region Correspondence between the various definitions of Anamorphism.

        public static IEnumerable<TResult> Infinite<TSource, TResult>(
            Func<TSource, TSource> iter,
            TSource seed,
            Func<TSource, TResult> resultSelector)
        {
            return Sequence.Create(_ => Iteration.Create(resultSelector.Invoke(_), iter.Invoke(_)), seed);
        }

        public static IEnumerable<TResult> Finite<TSource, TResult>(
            Func<TSource, TSource> iter,
            TSource seed,
            Func<TSource, TResult> resultSelector,
            Func<TSource, bool> predicate)
        {
            return Sequence.Create(
                _ => predicate.Invoke(_)
                    ? Iteration.MayCreate(resultSelector.Invoke(_), iter.Invoke(_))
                    : Maybe<Iteration<TResult, TSource>>.None,
                seed);
        }

        #endregion

        #region Generation Operators

        public static IEnumerable<int> Range(int start, int count)
        {
            return Sequence.Create(i => i + 1, 0, i => start + i, i => i < count);
        }

        public static IEnumerable<T> Repeat<T>(T value)
        {
            return Sequence.Create(i => i + 1, 0, i => value);
        }

        public static IEnumerable<T> Repeat<T>(T value, int count)
        {
            return Sequence.Create(i => i + 1, 0, i => value, i => i < count);
        }

        public static IEnumerable<T> Empty<T>()
        {
            return Sequence.Create(i => i, 0, i => default(T), i => false);
        }

        #endregion
    }
}
