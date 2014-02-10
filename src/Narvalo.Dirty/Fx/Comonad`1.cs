// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    // WARNING: Il s'agit d'une implémentation pour "démonstration".
    // On peut définir une comonade de deux manières équivalents :
    // - Counit & Cobind 
    // - Counit, Map & Comultiply
    sealed partial class Comonad<T>
    {
        // On utilise aussi Extend
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
#if COMONAD_VIA_MAP_COMULTIPLY
            throw new NotImplementedException();
#else
            return Cobind(_ => fun(ε(_)));
#endif
        }

        // Counité
        internal static T ε(Comonad<T> monad)
        {
            throw new NotImplementedException();
        }

        // Comultiplication
        internal static Comonad<Comonad<T>> δ(Comonad<T> monad)
        {
#if COMONAD_VIA_MAP_COMULTIPLY
            throw new NotImplementedException();
#else
            return monad.Cobind(_ => _);
#endif
        }
    }
}
