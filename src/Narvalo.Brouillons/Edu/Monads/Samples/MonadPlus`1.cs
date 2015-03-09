// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground.Edu.Monads.Samples
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public sealed class MonadPlus<T>
    {
        // [Haskell] mzero
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations",
            Justification = "This code is not meant to be used. Only for educational purpose.")]
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
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard naming convention from mathematics.")]
        internal static MonadPlus<T> η(T value)
        {
            throw new NotImplementedException();
        }

        // [Haskell] join
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard naming convention from mathematics.")]
        internal static MonadPlus<T> μ(MonadPlus<MonadPlus<T>> square)
        {
            return square.Bind(_ => _);
        }
    }
}
