// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

#define MONAD_VIA_BIND

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    sealed class Monad<T>
    {
        #region Monoid

        // [Haskell] mzero
        // The identity of mplus.
        public static Monad<T> Zero { get { throw new NotImplementedException(); } }

        // [Haskell] mplus
        // An associative operation.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "other",
            Justification = "Monad template definition.")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "Monad template definition.")]
        public Monad<T> Plus(Monad<T> other)
        {
            throw new NotImplementedException();
        }

        #endregion

        // [Haskell] >>=
        // Sequentially compose two actions, passing any value produced by the first as an argument to the second.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "kun",
            Justification = "Monad template definition.")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "Monad template definition.")]
        public Monad<TResult> Bind<TResult>(Kunc<T, TResult> kun)
        {
#if MONAD_VIA_BIND
            throw new NotImplementedException();
#else
            return Monad<TResult>.μ(Map(_ => kun.Invoke(_)));
#endif
        }

        // [Haskell] fmap
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "fun",
            Justification = "Monad template definition.")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "Monad template definition.")]
        public Monad<TResult> Map<TResult>(Func<T, TResult> fun)
        {
#if MONAD_VIA_BIND
            return Bind(_ => Monad<TResult>.η(fun.Invoke(_)));
#else
            throw new NotImplementedException();
#endif
        }

        // [Haskell] >>
        // Sequentially compose two actions, discarding any value produced by the first, 
        // like sequencing operators (such as the semicolon) in imperative languages.
        public Monad<TResult> Then<TResult>(Monad<TResult> other)
        {
            return Bind(_ => other);
        }

        // [Haskell] return
        // Inject a value into the monadic type.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value",
            Justification = "Monad template definition.")]
        internal static Monad<T> η(T value)
        {
            throw new NotImplementedException();
        }

        // [Haskell] join
        // The join function is the conventional monad join operator. It is used to remove 
        // one level of monadic structure, projecting its bound argument into the outer level.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "square",
            Justification = "Monad template definition.")]
        internal static Monad<T> μ(Monad<Monad<T>> square)
        {
#if MONAD_VIA_BIND
            return square.Bind(_ => _);
#else
            throw new NotImplementedException();
#endif
        }
    }
}
