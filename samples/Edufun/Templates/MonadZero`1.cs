// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Templates
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Applicative;

    public partial class MonadZero<T>
    {
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static MonadZero<T> Zero { get { throw new PrototypeException(); } }

        public MonadZero<TResult> Bind<TResult>(Func<T, MonadZero<TResult>> selector)
        {
            throw new PrototypeException();
        }

        internal static MonadZero<T> η(T value)
        {
            throw new PrototypeException();
        }

        internal static MonadZero<T> μ(MonadZero<MonadZero<T>> square)
            => square.Bind(Stubs<MonadZero<T>>.Identity);
    }
}
