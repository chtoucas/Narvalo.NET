// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell.Templates
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Fx;

    public partial class MonadPlus<T>
    {
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static MonadPlus<T> Zero { get { throw new NotImplementedException(); } }

        public MonadPlus<T> Plus(MonadPlus<T> other)
        {
            throw new NotImplementedException();
        }

        public MonadPlus<TResult> Bind<TResult>(Func<T, MonadPlus<TResult>> funM)
        {
            throw new NotImplementedException();
        }

        internal static MonadPlus<T> η(T value)
        {
            throw new NotImplementedException();
        }

        internal static MonadPlus<T> μ(MonadPlus<MonadPlus<T>> square)
        {
            return square.Bind(Stubs<MonadPlus<T>>.Identity);
        }
    }
}
