// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground.Edu.Monads.Samples
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public struct MonadValue<T> : IEquatable<MonadValue<T>>
        where T : struct
    {
        // [Haskell] mzero
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static MonadValue<T> None { get { throw new NotImplementedException(); } }

        // [Haskell] mplus
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "other")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public MonadValue<T> OrElse(MonadValue<T> other)
        {
            throw new NotImplementedException();
        }

        // [Haskell] >>=
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "funM")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public MonadValue<TResult> Bind<TResult>(Func<T, MonadValue<TResult>> funM)
        where TResult : struct
        {
            throw new NotImplementedException();
        }

        // [Haskell] return
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value")]
        internal static MonadValue<T> η(T value)
        {
            throw new NotImplementedException();
        }

        // [Haskell] join
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "square")]
        internal static MonadValue<T> μ(MonadValue<MonadValue<T>> square)
        {
            return square.Bind(_ => _);
        }

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public bool Equals(MonadValue<T> other)
        {
            throw new NotImplementedException();
        }

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "left")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "right")]
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static bool operator ==(MonadValue<T> left, MonadValue<T> right)
        {
            throw new NotImplementedException();
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "left")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "right")]
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static bool operator !=(MonadValue<T> left, MonadValue<T> right)
        {
            throw new NotImplementedException();
        }
    }
}
