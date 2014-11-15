// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground.Edu.Monads.Samples
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public sealed class Monad<T>
    {
        // [Haskell] >>=
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "funM")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public Monad<TResult> Bind<TResult>(Func<T, Monad<TResult>> funM)
        {
            throw new NotImplementedException();
        }

        // [Haskell] return
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value")]
        internal static Monad<T> η(T value)
        {
            throw new NotImplementedException();
        }

        // [Haskell] join
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "square")]
        internal static Monad<T> μ(Monad<Monad<T>> square)
        {
            return square.Bind(_ => _);
        }
    }
}
