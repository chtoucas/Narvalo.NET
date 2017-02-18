// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Templates
{
    using System;

    public sealed class Comonad<T>
    {
        public Comonad<TResult> Extend<TResult>(Func<Comonad<T>, TResult> fun)
        {
            throw new NotImplementedException();
        }

        public Comonad<TResult> Map<TResult>(Func<T, TResult> fun)
        {
            return Extend(_ => fun(ε(_)));
        }

        internal static T ε(Comonad<T> monad)
        {
            throw new NotImplementedException();
        }

        internal static Comonad<Comonad<T>> δ(Comonad<T> monad)
        {
            return monad.Extend(_ => _);
        }
    }
}
