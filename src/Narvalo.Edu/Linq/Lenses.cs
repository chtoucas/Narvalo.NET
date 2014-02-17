// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Linq
{
    using System;
    using System.Collections.Generic;

    public static class Lenses
    {
        public static IEnumerable<TResult> Ana<T, TResult>(
            Func<T, T> succ,
            T seed,
            Func<T, TResult> selector)
        {
            return Ana(succ, seed, predicate: _ => true, selector: selector);
        }

        public static IEnumerable<TResult> Ana<T, TResult>(
            Func<T, T> succ,
            T seed,
            Func<T, bool> predicate,
            Func<T, TResult> selector)
        {
            for (T value = seed; predicate.Invoke(value); value = succ.Invoke(value)) {
                yield return selector.Invoke(value);
            }
        }
    }
}
