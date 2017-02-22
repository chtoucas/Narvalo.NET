// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell
{
    using System;
    using System.Collections.Generic;

    public class KleisliOperators : IKleisliOperators
    {
        private static readonly IQueryOperators s_Qperators = new QueryOperators();

        public Monad<IEnumerable<TResult>> ForEach<TSource, TResult>(
            Func<TSource, Monad<TResult>> func,
            IEnumerable<TSource> seq)
        {
            return s_Qperators.SelectWith(seq, func);
        }

        public Func<TSource, Monad<TResult>> Compose<TSource, TMiddle, TResult>(
            Func<TSource, Monad<TMiddle>> first,
            Func<TMiddle, Monad<TResult>> second)
        {
            return _ => first.Invoke(_).Bind(second);
        }

        public Func<TSource, Monad<TResult>> ComposeBack<TSource, TMiddle, TResult>(
            Func<TMiddle, Monad<TResult>> first,
            Func<TSource, Monad<TMiddle>> second)
        {
            return _ => second.Invoke(_).Bind(first);
        }

        public Monad<TResult> Invoke<TSource, TResult>(
            Func<TSource, Monad<TResult>> func,
            Monad<TSource> value)
        {
            throw new NotImplementedException();
        }
    }
}