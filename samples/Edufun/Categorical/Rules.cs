// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical
{
    using System;

    public static partial class Rules
    {
        #region Monoid Laws

        /// <summary>
        /// First Monoid Law: Zero is a left identity for Plus.
        /// </summary>
        public static bool Monoid_FirstLaw<X>(Monad<X> m)
            => Monad<X>.Zero.Plus(m) == m;

        /// <summary>
        /// Second Monoid Law: Zero is a right identity for Plus.
        /// </summary>
        public static bool Monoid_SecondLaw<X>(Monad<X> m)
            => m.Plus(Monad<X>.Zero) == m;

        /// <summary>
        /// Third Monoid Law: Plus is associative.
        /// </summary>
        public static bool Monoid_ThirdLaw<X>(Monad<X> a, Monad<X> b, Monad<X> c)
            => a.Plus(b.Plus(c)) == (a.Plus(b)).Plus(c);

        #endregion

        #region Monad Laws

        /// <summary>
        /// First Monad Law: Unit is a left identity for Bind.
        /// </summary>
        public static bool Monad_FirstLaw<X, Y>(Kunc<X, Y> f, X value)
            => Monad.Return(value).Bind(f) == f(value);

        /// <summary>
        /// Second Monad Law: Unit is a right identity for Bind.
        /// </summary>
        public static bool Monad_SecondLaw<X>(Monad<X> m)
            => m.Bind(Monad.Return) == m;

        /// <summary>
        /// Third Monad Law: Bind is associative.
        /// </summary>
        public static bool Monad_ThirdLaw<X, Y, Z>(Monad<X> m, Kunc<X, Y> f, Kunc<Y, Z> g)
            => m.Bind(f).Bind(g) == m.Bind(_ => f(_).Bind(g));

        #endregion

        #region Monad Laws in the Kleisli Category

        /// <summary>
        /// First Monad Law: Return is a left identity for Compose.
        /// </summary>
        public static bool Monad_FirstLaw_Kleisli<X, Y>(Kunc<X, Y> g, X value)
        {
            Kunc<X, X> kReturn = Monad.Return;

            return kReturn.Compose(g).Invoke(value) == g(value);
        }

        /// <summary>
        /// Second Monad Law: Return is a right identity for Compose.
        /// </summary>
        public static bool Monad_SecondMonad_Kleisli<X, Y>(Kunc<X, Y> f, X value)
            => f.Compose(Monad.Return).Invoke(value) == f(value);

        /// <summary>
        /// Third Monad Law: Compose is associative.
        /// </summary>
        public static bool Monad_ThirdMonad_Kleisli<X, Y, Z, T>(
            Kunc<X, Y> f,
            Kunc<Y, Z> g,
            Kunc<Z, T> h,
            X value)
            => (f.Compose(g)).Compose(h).Invoke(value) == f.Compose(g.Compose(h)).Invoke(value);

        #endregion

        #region Functor Laws

        /// <summary>
        /// First Functor Law: The identity map is a fixed point for Map.
        /// </summary>
        public static bool Functor_FirstLaw<X>(Monad<X> m)
        {
            Func<Monad<X>, Monad<X>> idM = _ => _;

            return m.Map(_ => _) == idM.Invoke(m);
        }

        /// <summary>
        /// Second Functor Law: Map preserves the composition operator.
        /// </summary>
        public static bool Functor_SecondLaw<X, Y, Z>(Monad<X> m, Func<Y, Z> f, Func<X, Y> g)
            => m.Map(_ => f(g(_))) == m.Map(g).Map(f);

        #endregion

        /// <summary>
        /// Then is associative, implied by the definition of Then and the third monad law.
        /// </summary>
        public static bool ThenIsAssociative<X, Y, Z>(Monad<X> a, Monad<Y> b, Monad<Z> c)
            // (m >> n) >> o = m >> (n >> o)
            => a.Then(b).Then(c) == a.Then(b.Then(c));

#if !MONAD_DISABLE_ZERO

        /// <summary>
        /// MonadZero: Zero is a left zero for Bind.
        /// </summary>
        public static bool SatisfiesMonadZeroRule<X, Y>(Kunc<X, Y> f)
            // mzero >>= f = mzero
            => Monad<X>.Zero.Bind(f) == Monad<Y>.Zero;

        /// <summary>
        /// MonadMore: Zero is a right zero for Bind or equivalently Zero is a right zero for Then.
        /// </summary>
        public static bool SatisfiesMonadMoreRule<X>(Monad<X> m)
            // m >>= (\x -> mzero) = mzero
            => m.Bind(_ => Monad<X>.Zero) == Monad<X>.Zero;

        /// <summary>
        /// MonadMore: Zero is a right zero for Then, implied by the definition of then and the MonadMore rule.
        /// </summary>
        public static bool SatisfiesMonadMoreRuleVariant<X>(Monad<X> m)
            // v >> mzero = mzero
            => m.Then(Monad<X>.Zero) == Monad<X>.Zero;

#endif

#if !MONAD_DISABLE_ZERO && !MONAD_DISABLE_PLUS

        /// <summary>
        /// MonadPlus: Bind is right distributive over Plus.
        /// </summary>
        public static bool SatisfiesMonadPlusRule<X>(Monad<X> a, Monad<X> b, Kunc<X, X> f)
            // mplus a b >>= f = mplus (a >>= f) (b >>= f)
            => a.Plus(b).Bind(f) == a.Bind(f).Plus(b.Bind(f));

        /// <summary>
        /// MonadOr: Unit is a left zero for Plus.
        /// </summary>
        public static bool SatisfiesMonadOrRule<X>(X a, Monad<X> b)
            // morelse (return a) b ≡ return a
            => Monad.Return(a).Plus(b) == Monad.Return(a);

        /////// <summary>
        /////// Unit is a right zero for Plus.
        /////// </summary>
        ////public static bool Unused_UnitIsRightZeroForPlus<X>(Monad<X> a, X b)
        ////{
        ////    // morelse a (return b) ≡ return b
        ////    return a.Plus(Monad.Return(b)) == Monad.Return(b);
        ////}

#endif
    }
}
