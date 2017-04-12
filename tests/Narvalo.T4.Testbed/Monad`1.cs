// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.T4.Testbed
{
    using System;

    using Narvalo.Applicative;

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
            => square.Bind(Stubs<Monad<T>>.Ident);
    }
}
