// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell.Tmp
{
    using System;

    public class Mu
    {
        public Mu(Prototype<Mu> func)
        {
            Out = func;
        }

        public Prototype<Mu> Out { get; }

        public TResult Cata<TResult>(Func<Prototype<TResult>, TResult> phi)
            => phi(Out.Select(_ => _.Cata(phi)));

        public static Mu Ana<TResult>(
            TResult seed,
            Func<TResult, Prototype<TResult>> psi)
            => new Mu(psi(seed).Select(_ => Mu.Ana(_, psi)));

        public static TResult Hylo<TSource, TResult>(
            TSource seed,
            Func<TSource, Prototype<TSource>> psi,
            Func<Prototype<TResult>, TResult> phi)
            => Mu.Ana(seed, psi).Cata(phi);
    }
}
