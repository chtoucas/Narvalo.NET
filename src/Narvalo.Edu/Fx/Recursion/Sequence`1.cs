// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Fx.Recursion
{
    using System;

    public sealed class Sequence<T>
    {
        readonly Func<T> _iter;

        public Sequence(Func<T> iter)
        {
            _iter = iter;
        }

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
