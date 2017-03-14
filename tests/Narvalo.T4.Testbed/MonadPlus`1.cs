// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.T4.Testbed
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Applicative;

    public partial class MonadPlus<T>
    {
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static MonadPlus<T> Zero { get { throw new PrototypeException(); } }

        public MonadPlus<T> Plus(MonadPlus<T> other)
        {
            throw new PrototypeException();
        }

        public MonadPlus<TResult> Bind<TResult>(Func<T, MonadPlus<TResult>> selector)
        {
            throw new PrototypeException();
        }

        internal static MonadPlus<T> η(T value)
        {
            throw new PrototypeException();
        }

        internal static MonadPlus<T> μ(MonadPlus<MonadPlus<T>> square)
            => square.Bind(Stubs<MonadPlus<T>>.Identity);
    }
}
