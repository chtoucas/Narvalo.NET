// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell.Templates
{
    using System;

    using Narvalo.Fx;

    public sealed class Monad<T>
    {
        public Monad<TResult> Bind<TResult>(Func<T, Monad<TResult>> funM)
        {
            throw new NotImplementedException();
        }

        internal static Monad<T> η(T value)
        {
            throw new NotImplementedException();
        }

        internal static Monad<T> μ(Monad<Monad<T>> square)
        {
            // REVIEW: Why can't I use "_ => _" as with C# 5?
            return square.Bind(Stubs<Monad<T>>.Identity);
        }
    }
}
