// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Linq
{
    using System;
    using System.Collections.Generic;

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

    public abstract class Sequence<T>
    {
        protected Sequence() { }

        public abstract bool HasNext();

        public abstract T Next();

        public TResult Cata<TResult>(
            TResult seed,
            Func<TResult, bool> predicate,
            Func<TResult, T, TResult> accumulator)
        {
            TResult result = seed;

            while (predicate.Invoke(result) && HasNext()) {
                result = accumulator.Invoke(result, Next());
            }

            return result;
        }
    }

    public sealed class SimpleSequence<T> : Sequence<T>, IDisposable
    {
        readonly IEnumerator<T> _inner;

        public SimpleSequence(IEnumerable<T> list)
        {
            _inner = list.GetEnumerator();
        }

        public override bool HasNext()
        {
            return _inner.MoveNext();
        }

        public override T Next()
        {
            return _inner.Current;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public sealed class InfiniteSequence<T> : Sequence<T>
    {
        readonly Func<T> _iter;

        public InfiniteSequence(Func<T> iter)
        {
            _iter = iter;
        }

        public sealed override bool HasNext()
        {
            return true;
        }

        public override T Next()
        {
            return _iter.Invoke();
        }
    }
}
