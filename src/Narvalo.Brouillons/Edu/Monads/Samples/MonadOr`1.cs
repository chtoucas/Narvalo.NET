// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Monads.Samples
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public sealed class MonadOr<T>
    {
        // [Haskell] mzero
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations",
            Justification = "[Educational] This code is not meant to be used.")]
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
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Educational] Standard naming convention from mathematics.")]
        internal static MonadOr<T> η(T value)
        {
            throw new NotImplementedException();
        }

        // [Haskell] join
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Educational] Standard naming convention from mathematics.")]
        internal static MonadOr<T> μ(MonadOr<MonadOr<T>> square)
        {
            return square.Bind(_ => _);
        }
    }
}
