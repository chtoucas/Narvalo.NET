// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell.Templates
{
    using System;

    public sealed class Comonad<T>
    {
        public Comonad<TResult> Extend<TResult>(Func<Comonad<T>, TResult> func)
        {
            throw new PrototypeException();
        }

        public Comonad<TResult> Map<TResult>(Func<T, TResult> func) => Extend(_ => func(ε(_)));

        internal static T ε(Comonad<T> monad)
        {
            throw new PrototypeException();
        }

        internal static Comonad<Comonad<T>> δ(Comonad<T> monad) => monad.Extend(_ => _);
    }
}
