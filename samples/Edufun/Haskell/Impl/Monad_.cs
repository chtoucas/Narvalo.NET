// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

//#define MONAD_VIA_MAP_MULTIPLY

namespace Edufun.Haskell.Impl
{
    using System;

    using Narvalo.Fx;

    public static class Monad
    {
        public static Monad<Unit> Unit => Of(Narvalo.Fx.Unit.Single);

        public static Monad<T> Of<T>(T value) { throw new FakeClassException(); }

        public static Monad<T> Flatten<T>(Monad<Monad<T>> square)
        {
#if MONAD_VIA_MAP_MULTIPLY
            throw new FakeClassException();
#else
            Func<Monad<T>, Monad<T>> id = _ => _;

            return square.Bind(id);
#endif
        }
    }

    public partial class Monad<T> : IMonad<T>, IMonadSyntax<T>
    {
        public Monad<TResult> Bind<TResult>(Func<T, Monad<TResult>> selector)
        {
#if MONAD_VIA_MAP_MULTIPLY
            return Monad.Flatten(Select(_ => selector.Invoke(_)));
#else
            throw new FakeClassException();
#endif
        }

        // m >>= return  =  m
        public Monad<TResult> Select<TResult>(Func<T, TResult> selector)
        {
#if MONAD_VIA_MAP_MULTIPLY
            throw new FakeClassException();
#else
            return Bind(_ => Monad.Of(selector.Invoke(_)));
#endif
        }

        public Monad<TSource> Of_<TSource>(TSource value) => Monad.Of(value);

        public Monad<TSource> Flatten_<TSource>(Monad<Monad<TSource>> square) => Monad.Flatten(square);
    }
}
