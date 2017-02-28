// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell.Templates
{
    using System;

    using Narvalo.Fx;

    public partial class Monad<T>
    {
        public Monad<TResult> Bind<TResult>(Func<T, Monad<TResult>> selector)
        {
            throw new PrototypeException();
        }

        internal static Monad<T> η(T value)
        {
            throw new PrototypeException();
        }

        internal static Monad<T> μ(Monad<Monad<T>> square)
            => square.Bind(Stubs<Monad<T>>.Identity);
    }
}
