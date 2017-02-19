// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical
{
    using System;

    public class Mu
    {
        public Mu(Monad<Mu> fun)
        {
            Out = fun;
        }

        public Monad<Mu> Out { get; }

        public static Mu Ana<TResult>(Func<TResult, Monad<TResult>> psi, TResult seed)
            => new Mu(psi(seed).Select(_ => Mu.Ana(psi, _)));

        public static T2 Hylo<T1, T2>(
            Func<T1, Monad<T1>> psi,
            Func<Monad<T2>, T2> phi,
            T1 seed)
            => Mu.Ana(psi, seed).Cata(phi);

        public TResult Cata<TResult>(Func<Monad<TResult>, TResult> phi)
            => phi(Out.Select(_ => _.Cata(phi)));
    }
}
