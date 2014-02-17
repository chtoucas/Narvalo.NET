// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Linq
{
    using System;
    using System.Collections.Generic;

    // Exploring http://blogs.bartdesmet.net/blogs/bart/archive/2010/01/01/the-essence-of-linq-minlinq.aspx

    /* 
     * Anamorphism (Lenses)
     * --------------------
     * T -> ... -> M<TResult>
     * (T -> T) -> T -> (T -> bool) -> (T --> TResult) -> M<TResult>
     * 
     * Lenses [(...)]
     * 
     * Zero, Return, Empty, Repeat 
     */

    static class Lenses
    {
        public static IEnumerable<TResult> Ana<T, TResult>(
            Func<T, T> next,
            T seed,
            Func<T, bool> predicate,
            Func<T, TResult> selector)
        {
            for (T t = seed; predicate.Invoke(t); t = next.Invoke(t)) {
                yield return selector.Invoke(t);
            }
        }

        /* 
         * Bind
         * ----
         * M<T> -> (T -> bool) -> (T -> M<C>) -> (T -> C -> TResult) -> M<TResult>
         */
    }
}
