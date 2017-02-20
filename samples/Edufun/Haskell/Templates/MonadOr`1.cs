// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell.Templates
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Fx;

    public sealed class MonadOr<T>
    {
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static MonadOr<T> None { get { throw new NotImplementedException(); } }

        public MonadOr<T> OrElse(MonadOr<T> other)
        {
            throw new NotImplementedException();
        }

        public MonadOr<TResult> Bind<TResult>(Func<T, MonadOr<TResult>> funM)
        {
            throw new NotImplementedException();
        }

        internal static MonadOr<T> η(T value)
        {
            throw new NotImplementedException();
        }

        internal static MonadOr<T> μ(MonadOr<MonadOr<T>> square)
        {
            return square.Bind(Stubs<MonadOr<T>>.Identity);
        }
    }
}
