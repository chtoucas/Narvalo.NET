// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell.Impl
{
    using System;
    using System.Collections.Generic;

    public class KleisliOperators : IKleisliOperators
    {
        private static readonly IQueryOperators s_Qperators = new QueryOperators();

        public Monad<IEnumerable<TResult>> ForEach<TSource, TResult>(
            Func<TSource, Monad<TResult>> me,
            IEnumerable<TSource> seq)
        {
            return s_Qperators.SelectWith(seq, me);
        }

        public Func<TSource, Monad<TResult>> Compose<TSource, TMiddle, TResult>(
            Func<TSource, Monad<TMiddle>> me,
            Func<TMiddle, Monad<TResult>> thunk)
        {
            return _ => me.Invoke(_).Bind(thunk);
        }

        public Func<TSource, Monad<TResult>> ComposeBack<TSource, TMiddle, TResult>(
            Func<TMiddle, Monad<TResult>> me,
            Func<TSource, Monad<TMiddle>> thunk)
        {
            return _ => thunk.Invoke(_).Bind(me);
        }

        public Monad<TResult> Invoke<TSource, TResult>(
            Func<TSource, Monad<TResult>> me,
            Monad<TSource> value)
        {
            throw new NotImplementedException();
        }
    }
}