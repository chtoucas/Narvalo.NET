// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground.Edu.Monads.Samples
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public sealed class MonadOr<T>
    {
        // [Haskell] mzero
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static MonadOr<T> None { get { throw new NotImplementedException(); } }

        // [Haskell] mplus
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "other")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public MonadOr<T> OrElse(MonadOr<T> other)
        {
            throw new NotImplementedException();
        }

        // [Haskell] >>=
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "funM")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public MonadOr<TResult> Bind<TResult>(Func<T, MonadOr<TResult>> funM)
        {
            throw new NotImplementedException();
        }

        // [Haskell] return
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value")]
        internal static MonadOr<T> η(T value)
        {
            throw new NotImplementedException();
        }

        // [Haskell] join
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "square")]
        internal static MonadOr<T> μ(MonadOr<MonadOr<T>> square)
        {
            return square.Bind(_ => _);
        }
    }
}
