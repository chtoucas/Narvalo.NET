// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Samples
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public sealed class MonadZero<T>
    {
        // [Haskell] mzero
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = "[Intentionally] This code is not meant to be used.")]
        public static MonadZero<T> Zero { get { throw new NotImplementedException(); } }

        // [Haskell] >>=
        public MonadZero<TResult> Bind<TResult>(Func<T, MonadZero<TResult>> funM)
        {
            throw new NotImplementedException();
        }

        // [Haskell] return
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "[Intentionally] Standard naming convention from mathematics.")]
        internal static MonadZero<T> η(T value)
        {
            throw new NotImplementedException();
        }

        // [Haskell] join
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "[Intentionally] Standard naming convention from mathematics.")]
        internal static MonadZero<T> μ(MonadZero<MonadZero<T>> square)
        {
            return square.Bind(Stubs<MonadZero<T>>.Identity);
        }
    }
}
