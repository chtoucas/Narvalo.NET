// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

#define COMONAD_VIA_COBIND

namespace Narvalo.Fx.Skeleton
{
    using System;

    sealed class Comonad<T>
    {
        public Comonad<TResult> Cobind<TResult>(Cokunc<T, TResult> cokun)
        {
#if COMONAD_VIA_COBIND
            throw new NotImplementedException();
#else
            return δ(this).Map(_ => cokun.Invoke(_));
#endif
        }

        public Comonad<TResult> Map<TResult>(Func<T, TResult> fun)
        {
#if COMONAD_VIA_COBIND
            return Cobind(_ => fun(ε(_)));
#else
            throw new NotImplementedException();
#endif
        }

        internal static T ε(Comonad<T> monad)
        {
            throw new NotImplementedException();
        }

        internal static Comonad<Comonad<T>> δ(Comonad<T> monad)
        {
#if COMONAD_VIA_COBIND
            return monad.Cobind(_ => _);
#else
            throw new NotImplementedException();
#endif
        }
    }
}
