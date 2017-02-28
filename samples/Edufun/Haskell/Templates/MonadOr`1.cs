// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell.Templates
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Fx;

    public partial class MonadOr<T>
    {
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static MonadOr<T> None { get { throw new PrototypeException(); } }

        public MonadOr<T> OrElse(MonadOr<T> other)
        {
            throw new PrototypeException();
        }

        public MonadOr<TResult> Bind<TResult>(Func<T, MonadOr<TResult>> selector)
        {
            throw new PrototypeException();
        }

        internal static MonadOr<T> η(T value)
        {
            throw new PrototypeException();
        }

        internal static MonadOr<T> μ(MonadOr<MonadOr<T>> square)
            => square.Bind(Stubs<MonadOr<T>>.Identity);
    }
}
