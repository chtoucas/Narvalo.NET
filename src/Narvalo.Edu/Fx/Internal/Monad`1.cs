// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Fx.Internal
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    sealed class Monad<T>
    {
#if !MONAD_DISABLE_ZERO
        // [Haskell] mzero
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static Monad<T> Zero { get { throw new NotImplementedException(); } }

#if !MONAD_DISABLE_PLUS
        // [Haskell] mplus
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "other")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public Monad<T> Plus(Monad<T> other)
        {
            throw new NotImplementedException();
        }
#endif
#endif

        // [Haskell] >>=
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "kun")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public Monad<TResult> Bind<TResult>(Kunc<T, TResult> kun)
        {
#if MONAD_VIA_MAP_MULTIPLY
            return Monad<TResult>.μ(Map(_ => kun.Invoke(_)));
#else
            throw new NotImplementedException();
#endif
        }

        // [Haskell] fmap
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "fun")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public Monad<TResult> Map<TResult>(Func<T, TResult> selector)
        {
#if MONAD_VIA_MAP_MULTIPLY
            throw new NotImplementedException();
#else
            return Bind(_ => Monad<TResult>.η(selector.Invoke(_)));
#endif
        }

        // [Haskell] >>
        public Monad<TResult> Then<TResult>(Monad<TResult> other)
        {
            return Bind(_ => other);
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
#if MONAD_VIA_MAP_MULTIPLY
            throw new NotImplementedException();
#else
            return square.Bind(_ => _);
#endif
        }
    }
}
