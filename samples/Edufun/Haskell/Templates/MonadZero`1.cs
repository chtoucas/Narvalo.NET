// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell.Templates
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Fx;

    public sealed class MonadZero<T>
    {
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static MonadZero<T> Zero { get { throw new NotImplementedException(); } }

        public MonadZero<TResult> Bind<TResult>(Func<T, MonadZero<TResult>> funM)
        {
            throw new NotImplementedException();
        }

        internal static MonadZero<T> η(T value)
        {
            throw new NotImplementedException();
        }

        internal static MonadZero<T> μ(MonadZero<MonadZero<T>> square)
        {
            return square.Bind(Stubs<MonadZero<T>>.Identity);
        }
    }
}
