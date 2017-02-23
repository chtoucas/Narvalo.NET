// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

//#define COMONAD_VIA_MAP_COMULTIPLY

namespace Edufun.Haskell.Tmp
{
    using System;

    public sealed class Comonad<T>
    {
        public Comonad<TResult> Extend<TResult>(Cokunc<T, TResult> cokun)
        {
#if COMONAD_VIA_MAP_COMULTIPLY
            return Duplicate(this).Map(_ => cokun.Invoke(_));
#else
            throw new PrototypeException();
#endif
        }

        public Comonad<TResult> Select<TResult>(Func<T, TResult> fun)
        {
#if COMONAD_VIA_MAP_COMULTIPLY
            throw new PrototypeException();
#else
            return Extend(_ => fun(Extract(_)));
#endif
        }

        // ε
        internal static T Extract(Comonad<T> value)
        {
            throw new PrototypeException();
        }

        // δ
        internal static Comonad<Comonad<T>> Duplicate(Comonad<T> value)
        {
#if COMONAD_VIA_MAP_COMULTIPLY
            throw new PrototypeException();
#else
            return value.Extend(_ => _);
#endif
        }
    }

    public static class Comonad
    {
        public static T Extract<T>(Comonad<T> value) => Comonad<T>.Extract(value);

        public static Comonad<Comonad<T>> Duplicate<T>(Comonad<T> value) => Comonad<T>.Duplicate(value);
    }
}
