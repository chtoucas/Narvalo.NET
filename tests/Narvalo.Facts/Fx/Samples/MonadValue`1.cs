// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Samples
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public struct MonadValue<T> : IEquatable<MonadValue<T>>
        where T : struct
    {
        // [Haskell] mzero
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations",
            Justification = "[Educational] This code is not meant to be used.")]
        public static MonadValue<T> None { get { throw new NotImplementedException(); } }

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations",
            Justification = "[Educational] This code is not meant to be used.")]
        public static bool operator ==(MonadValue<T> left, MonadValue<T> right)
        {
            throw new NotImplementedException();
        }

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations",
            Justification = "[Educational] This code is not meant to be used.")]
        public static bool operator !=(MonadValue<T> left, MonadValue<T> right)
        {
            throw new NotImplementedException();
        }

        // [Haskell] mplus
        public MonadValue<T> OrElse(MonadValue<T> other)
        {
            throw new NotImplementedException();
        }

        // [Haskell] >>=
        public MonadValue<TResult> Bind<TResult>(Func<T, MonadValue<TResult>> funM)
            where TResult : struct
        {
            throw new NotImplementedException();
        }

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations",
            Justification = "[Educational] This code is not meant to be used.")]
        public bool Equals(MonadValue<T> other)
        {
            throw new NotImplementedException();
        }

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations",
            Justification = "[Educational] This code is not meant to be used.")]
        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations",
            Justification = "[Educational] This code is not meant to be used.")]
        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        // [Haskell] return
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Educational] Standard naming convention from mathematics.")]
        internal static MonadValue<T> η(T value)
        {
            throw new NotImplementedException();
        }

        // [Haskell] join
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Educational] Standard naming convention from mathematics.")]
        internal static MonadValue<T> μ(MonadValue<MonadValue<T>> square)
        {
            return square.Bind(_ => _);
        }
    }
}
