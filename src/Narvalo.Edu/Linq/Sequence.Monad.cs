// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Linq
{
    using System;
    using Narvalo.Edu.Fx;

    public static partial class Sequence
    {
        public static Monad<TResult> CataM<T, TResult>(
            Func<T> iter,
            TResult seed,
            Func<Monad<TResult>, bool> predicate,
            Kunc<TResult, T, TResult> accumulatorM)
        {
            Monad<TResult> result = Monad.Return(seed);

            while (predicate.Invoke(result)) {
                result = result.Bind(_ => accumulatorM.Invoke(_, iter.Invoke()));
            }

            return result;
        }

        #region Aggregate Operators

        public static Monad<TResult> AggregateM<T, TResult>(
            Func<T> iter,
            TResult seed,
            Kunc<TResult, T, TResult> accumulatorM)
        {
            return CataM(iter, seed, _ => true, accumulatorM);
        }

        #endregion
    }
}
