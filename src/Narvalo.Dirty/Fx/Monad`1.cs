// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    /* WARNING: This is not meant to be a generic monad implementation.
     *
     * There are two equivalent ways to define a monad:
     * - Haskell: Unit, Bind
     * - Category: Unit, Map, Multiply
     *
     * Aliases:
     *
     * Name     | Maths | Haskell | What we will use
     * ---------+-------+---------+
     * Unit     | η     | Return  | Return (NB: sometimes we prefer to use a more appropriate name, like Create)
     * Bind     |       | Bind    | Bind
     * Map      |       | Fmap    | Map    (NB: this is the very Select of Linq. In Haskell this is also called LiftM)
     * Multiply | μ     | Join    | Join
     * 
     * Additional methods
     *          |       | LiftM2  | Zip
     *          
     * For MonadPlus        
     *          |       | Mplus   | Plus
     *
     * NB: For the purpose of demonstration we define an additive monad (a MonadPlus): 
     * - Zero.Plus(other) = other
     * - other.Plus(Zero) = other
     * or
     * - Zero.Bind(kun) = Zero
     * - other.Bind(_ => Zero) = Zero
     * TODO: Match? Fail? Sum, Guard for MonadPlus? Then (Bind) and Where
     */

    sealed partial class Monad<T>
    {
        public static Monad<T> Zero { get { throw new NotImplementedException(); } }

        public Monad<T> Plus(Monad<T> other)
        {
            throw new NotImplementedException();
        }

        public Monad<TResult> Bind<TResult>(Kunc<T, TResult> kun)
        {
#if MONAD_VIA_BIND
            throw new NotImplementedException();
#else
            return Monad<TResult>.μ(Map(_ => kun.Invoke(_)));
#endif
        }

        public Monad<TResult> Map<TResult>(Func<T, TResult> fun)
        {
#if MONAD_VIA_MAP_MULTIPLY
            throw new NotImplementedException();
#else
            return Bind(_ => Monad<TResult>.η(fun.Invoke(_)));
#endif
        }

        internal static Monad<T> η(T value)
        {
            throw new NotImplementedException();
        }

        internal static Monad<T> μ(Monad<Monad<T>> square)
        {
#if MONAD_VIA_MAP_MULTIPLY
            throw new NotImplementedException();
#else
            return square.Bind(_ => _);
#endif
        }
    }
}
