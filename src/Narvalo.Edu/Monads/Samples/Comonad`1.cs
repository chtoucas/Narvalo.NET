// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Monads.Samples
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public sealed class Comonad<T>
    {
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "fun")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public Comonad<TResult> Extend<TResult>(Func<Comonad<T>, TResult> fun)
        {
            throw new NotImplementedException();
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "fun")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public Comonad<TResult> Map<TResult>(Func<T, TResult> fun)
        {
            return Extend(_ => fun(ε(_)));
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "monad")]
        internal static T ε(Comonad<T> monad)
        {
            throw new NotImplementedException();
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "monad")]
        internal static Comonad<Comonad<T>> δ(Comonad<T> monad)
        {
            return monad.Extend(_ => _);
        }
    }
}
