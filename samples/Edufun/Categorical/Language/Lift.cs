// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    using System;

    public partial class Monad
    {
        public Func<Monad<T>, Monad<TResult>> Lift<T, TResult>(Func<T, TResult> thunk)
        {
            throw new NotImplementedException();
        }

        public Func<Monad<T1>, Monad<T2>, Monad<TResult>> Lift<T1, T2, TResult>(Func<T1, T2, TResult> thunk)
        {
            throw new NotImplementedException();
        }

        public Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<TResult>> Lift<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> thunk)
        {
            throw new NotImplementedException();
        }

        public Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<TResult>> Lift<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> thunk)
        {
            throw new NotImplementedException();
        }

        public Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<T5>, Monad<TResult>> Lift<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> thunk)
        {
            throw new NotImplementedException();
        }
    }
}