// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;

    public static class Sequence
    {
        #region Anamorphims

        public static IEnumerable<TResult> Create<TSource, TResult>(
            Func<TSource, TSource> iter,
            TSource seed,
            Func<TSource, TResult> resultSelector)
        {
            return Create(iter, seed, resultSelector, _ => true);
        }

        public static IEnumerable<TResult> Create<TSource, TResult>(
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

        #endregion
    }
}
