// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    /* WARNING: This is not meant to be a generic monad implementation.
     *
     * There are two equivalent ways to define a comonad:
     * - Haskell: Counit, Cobind (Extend)
     * - Category: Counit, Map, Comultiply (δ)
     *
     * Aliases:
     *
      * Name       | Maths | Haskell   | HERE
     * -----------+-------+-----------+
     * Counit     | ε     | Extract   | Extract
     * Cobind     |       | Extend    | Cobind
     * Map        |       |           | Map
     * Comultiply | δ     | Duplicate | Duplicate
     */

    sealed partial class Comonad<T>
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
#if COMONAD_VIA_MAP_COMULTIPLY
            throw new NotImplementedException();
#else
            return Cobind(_ => fun(ε(_)));
#endif
        }

        internal static T ε(Comonad<T> monad)
        {
            throw new NotImplementedException();
        }

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
