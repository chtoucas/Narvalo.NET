// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Templates
{
    using System;

    using Narvalo.Fx;

    public sealed class MonadPlus<T>
    {
        // [Haskell] mzero
        public static MonadPlus<T> Zero { get { throw new NotImplementedException(); } }

        // [Haskell] mplus
        public MonadPlus<T> Plus(MonadPlus<T> other)
        {
            throw new NotImplementedException();
        }

        // [Haskell] >>=
        public MonadPlus<TResult> Bind<TResult>(Func<T, MonadPlus<TResult>> funM)
        {
            throw new NotImplementedException();
        }

        // [Haskell] return
        internal static MonadPlus<T> η(T value)
        {
            throw new NotImplementedException();
        }

        // [Haskell] join
        internal static MonadPlus<T> μ(MonadPlus<MonadPlus<T>> square)
        {
            return square.Bind(Stubs<MonadPlus<T>>.Identity);
        }
    }
}
