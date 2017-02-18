// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Monads
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public sealed class Monad<T>
    {
        // [Haskell] mzero
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations",
            Justification = "[Educational] This code is not meant to be used.")]
        public static Monad<T> Zero { get { throw new NotImplementedException(); } }

        // [Haskell] mplus
        public Monad<T> Plus(Monad<T> other)
        {
            throw new NotImplementedException();
        }

        // [Haskell] >>=
        public Monad<TResult> Bind<TResult>(Kunc<T, TResult> kun)
        {
#if MONAD_VIA_MAP_MULTIPLY
            return Monad<TResult>.μ(Map(_ => kun.Invoke(_)));
#else
            throw new NotImplementedException();
#endif
        }

        // [Haskell] fmap
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
            => Bind(_ => other);

        // [Haskell] return
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Educational] Standard naming convention from mathematics.")]
        internal static Monad<T> η(T value)
        {
            throw new NotImplementedException();
        }

        // [Haskell] join
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Educational] Standard naming convention from mathematics.")]
        internal static Monad<T> μ(Monad<Monad<T>> square)
        {
#if MONAD_VIA_MAP_MULTIPLY
            throw new NotImplementedException();
#else
            Kunc<Monad<T>, T> id = _ => _;

            return square.Bind(id);
#endif
        }
    }
}
