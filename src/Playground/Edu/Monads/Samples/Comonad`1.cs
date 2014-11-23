// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground.Edu.Monads.Samples
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public sealed class Comonad<T>
    {
        public Comonad<TResult> Extend<TResult>(Func<Comonad<T>, TResult> fun)
        {
            throw new NotImplementedException();
        }

        public Comonad<TResult> Map<TResult>(Func<T, TResult> fun)
        {
            return Extend(_ => fun(ε(_)));
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard naming convention from mathematics.")]
        internal static T ε(Comonad<T> monad)
        {
            throw new NotImplementedException();
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard naming convention from mathematics.")]
        internal static Comonad<Comonad<T>> δ(Comonad<T> monad)
        {
            return monad.Extend(_ => _);
        }
    }
}
