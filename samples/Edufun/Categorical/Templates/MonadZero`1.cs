// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Templates
{
    using System;

    using Narvalo.Fx;

    public sealed class MonadZero<T>
    {
        // [Haskell] mzero
        public static MonadZero<T> Zero { get { throw new NotImplementedException(); } }

        // [Haskell] >>=
        public MonadZero<TResult> Bind<TResult>(Func<T, MonadZero<TResult>> funM)
        {
            throw new NotImplementedException();
        }

        // [Haskell] return
        internal static MonadZero<T> η(T value)
        {
            throw new NotImplementedException();
        }

        // [Haskell] join
        internal static MonadZero<T> μ(MonadZero<MonadZero<T>> square)
        {
            return square.Bind(Stubs<MonadZero<T>>.Identity);
        }
    }
}
