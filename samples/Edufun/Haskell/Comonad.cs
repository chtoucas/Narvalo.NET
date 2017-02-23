// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

//#define COMONAD_VIA_MAP_COMULTIPLY

namespace Edufun.Haskell
{
    using System;

    public class Comonad<T>
    {
        public Comonad<TResult> Extend<TResult>(Func<Comonad<T>, TResult> func)
        {
#if COMONAD_VIA_MAP_COMULTIPLY
            return Duplicate(this).Select(func);
#else
            throw new PrototypeException();
#endif
        }

        public Comonad<TResult> Select<TResult>(Func<T, TResult> func)
        {
#if COMONAD_VIA_MAP_COMULTIPLY
            throw new PrototypeException();
#else
            return Extend(_ => func(Extract(_)));
#endif
        }

        public static T Extract(Comonad<T> value)
        {
            throw new PrototypeException();
        }

        public static Comonad<Comonad<T>> Duplicate(Comonad<T> value)
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
