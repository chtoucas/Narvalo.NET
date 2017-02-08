// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Monads
{
    using System;

    using Narvalo.Fx;

    public struct MonadValue<T> : IEquatable<MonadValue<T>>
        where T : struct
    {
        // [Haskell] mzero
        public static MonadValue<T> None { get { throw new NotImplementedException(); } }

        public static bool operator ==(MonadValue<T> left, MonadValue<T> right)
        {
            throw new NotImplementedException();
        }

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

        public bool Equals(MonadValue<T> other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        // [Haskell] return
        internal static MonadValue<T> η(T value)
        {
            throw new NotImplementedException();
        }

        // [Haskell] join
        internal static MonadValue<T> μ(MonadValue<MonadValue<T>> square)
        {
            return square.Bind(Stubs<MonadValue<T>>.Identity);
        }
    }
}
