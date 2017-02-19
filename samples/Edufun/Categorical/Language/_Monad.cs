// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

//#define MONAD_VIA_MAP_MULTIPLY

namespace Edufun.Categorical.Language
{
    using System;

    public partial class Monad : IMonad, IMonadGrammar
    {
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

        public Monad<T> Pure<T>(T value) => Of(value);

        public Monad<T> Join<T>(Monad<Monad<T>> square) => Flatten(square);
    }

    public partial class Monad<T> : IMonad<T>, IMonadGrammar<T>
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
    }
}
