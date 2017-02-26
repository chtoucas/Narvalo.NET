// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell.Templates
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Fx;

    public partial struct MonadValue<T> : IEquatable<MonadValue<T>> where T : struct
    {
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static MonadValue<T> None { get { throw new PrototypeException(); } }

        public MonadValue<T> OrElse(MonadValue<T> other)
        {
            throw new PrototypeException();
        }

        public MonadValue<TResult> Bind<TResult>(Func<T, MonadValue<TResult>> funM)
            where TResult : struct
        {
            throw new PrototypeException();
        }

        public static bool operator ==(MonadValue<T> left, MonadValue<T> right) => left.Equals(right);

        public static bool operator !=(MonadValue<T> left, MonadValue<T> right) => !left.Equals(right);

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public bool Equals(MonadValue<T> other)
        {
            throw new PrototypeException();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MonadValue<T>)) { return false; }

            return Equals((MonadValue<T>)obj);
        }

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public override int GetHashCode()
        {
            throw new PrototypeException();
        }

        internal static MonadValue<T> η(T value)
        {
            throw new PrototypeException();
        }

        internal static MonadValue<T> μ(MonadValue<MonadValue<T>> square)
        {
            return square.Bind(Stubs<MonadValue<T>>.Identity);
        }
    }
}
