// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Monads
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public sealed class Comonad<T>
    {
        public Comonad<TResult> Extend<TResult>(Cokunc<T, TResult> cokun)
        {
#if COMONAD_VIA_MAP_COMULTIPLY
            return δ(this).Map(_ => cokun.Invoke(_));
#else
            throw new NotImplementedException();
#endif
        }

        public Comonad<TResult> Map<TResult>(Func<T, TResult> fun)
        {
#if COMONAD_VIA_MAP_COMULTIPLY
            throw new NotImplementedException();
#else
            return Extend(_ => fun(ε(_)));
#endif
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Educational] Standard naming convention from mathematics.")]
        internal static T ε(Comonad<T> monad)
        {
            throw new NotImplementedException();
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Educational] Standard naming convention from mathematics.")]
        internal static Comonad<Comonad<T>> δ(Comonad<T> monad)
        {
#if COMONAD_VIA_MAP_COMULTIPLY
            throw new NotImplementedException();
#else
            return monad.Extend(_ => _);
#endif
        }
    }
}
