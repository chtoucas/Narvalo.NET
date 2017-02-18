// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Templates
{
    using System;

    using Narvalo.Fx;

    public sealed class MonadOr<T>
    {
        // [Haskell] mzero
        public static MonadOr<T> None { get { throw new NotImplementedException(); } }

        // [Haskell] mplus
        public MonadOr<T> OrElse(MonadOr<T> other)
        {
            throw new NotImplementedException();
        }

        // [Haskell] >>=
        public MonadOr<TResult> Bind<TResult>(Func<T, MonadOr<TResult>> funM)
        {
            throw new NotImplementedException();
        }

        // [Haskell] return
        internal static MonadOr<T> η(T value)
        {
            throw new NotImplementedException();
        }

        // [Haskell] join
        internal static MonadOr<T> μ(MonadOr<MonadOr<T>> square)
        {
            return square.Bind(Stubs<MonadOr<T>>.Identity);
        }
    }
}
