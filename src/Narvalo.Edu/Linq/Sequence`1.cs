// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Linq
{
    using System;

    // Exploring http://blogs.bartdesmet.net/blogs/bart/archive/2010/01/01/the-essence-of-linq-minlinq.aspx

    /*!
     * Catamorphism (Bananas)
     * ----------------------
     * M<T> -> ... -> TResult
     * M<T> -> TResult -> (TResult -> bool) -> (TResult -> T -> TResult) -> TResult
     * 
     * Bananas (|...|)
     * 
     * Anamorphism (Lenses)
     * --------------------
     * T -> ... -> M<TResult>
     * (T -> T) -> T -> (T -> bool) -> (T --> TResult) -> M<TResult>
     * 
     * Lenses [(...)]
     * 
     * Zero, Return, Empty, Repeat 
     * 
     * Bind
     * ----
     * M<T> -> (T -> bool) -> (T -> M<C>) -> (T -> C -> TResult) -> M<TResult>
     */

    public sealed class Sequence<T>
    {
        readonly Func<T> _iter;

        public Sequence(Func<T> iter)
        {
            _iter = iter;
        }

        internal Func<T> Iterator { get { return _iter; } }

        public TResult Cata<TResult>(
            TResult seed,
            Func<TResult, bool> predicate,
            Func<TResult, T, TResult> accumulator)
        {
            TResult result = seed;

            while (predicate.Invoke(result)) {
                result = accumulator.Invoke(result, _iter.Invoke());
            }

            return result;
        }
    }
}
