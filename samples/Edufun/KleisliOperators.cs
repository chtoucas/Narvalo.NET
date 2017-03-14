// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun
{
    using System;
    using System.Collections.Generic;

    public class KleisliOperators : IKleisliOperators
    {
        private static readonly IQueryOperators s_Qperators = new QueryOperators();

        // [Control.Monad] f >=> g = \x -> f x >>= g
        public Func<TSource, Prototype<TResult>> Compose<TSource, TMiddle, TResult>(
            Func<TSource, Prototype<TMiddle>> first,
            Func<TMiddle, Prototype<TResult>> second)
            => arg => first.Invoke(arg).Bind(second);

        // [Control.Monad] (<=<) = flip (>=>)
        public Func<TSource, Prototype<TResult>> ComposeBack<TSource, TMiddle, TResult>(
            Func<TMiddle, Prototype<TResult>> first,
            Func<TSource, Prototype<TMiddle>> second)
            => arg => second.Invoke(arg).Bind(first);

        // [GHC.Base] f =<< x = x >>= f
        public Prototype<TResult> InvokeWith<TSource, TResult>(
            Func<TSource, Prototype<TResult>> func,
            Prototype<TSource> value)
            => value.Bind(func);

        // [Data.Traversable] forM = flip mapM
        public Prototype<IEnumerable<TResult>> InvokeWith<TSource, TResult>(
            Func<TSource, Prototype<TResult>> func,
            IEnumerable<TSource> seq)
            => s_Qperators.SelectWith(seq, func);
    }
}