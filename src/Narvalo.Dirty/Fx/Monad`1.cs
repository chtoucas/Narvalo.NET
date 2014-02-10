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
     * Name     | Maths | Haskell | HERE
     * ---------+-------+---------+
     * Unit     | η     | Return  | Return (or a more appropriate name)
     * Bind     |       | Bind    | Bind
     * Map      |       | LiftM   | Map
     * Multiply | μ     |         | Join
     *          |       | LiftM2  | Zip
     *
     * NB: This is an additive monad + ValueOrElse
     */

    sealed partial class Monad<T>
    {
        public static Monad<T> Zero { get { throw new NotImplementedException(); } }

        // REVIEW
        internal T ExtractOrElse(T defaultValue)
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
