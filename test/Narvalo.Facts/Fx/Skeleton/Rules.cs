// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Skeleton
{
    public static class Rules
    {
        #region Monoid Laws

        /// <summary>
        /// First Monoid Law: Zero is a left identity for Plus.
        /// </summary>
        static bool Monoid_FirstLaw<X>(Monad<X> monad)
        {
            // mplus mzero m = m
            return Monad<X>.Zero.Plus(monad) == monad;
        }

        /// <summary>
        /// Second Monoid Law: Zero is a right identity for Plus.
        /// </summary>
        static bool Monoid_SecondLaw<X>(Monad<X> monad)
        {
            // mplus m mzero = m
            return monad.Plus(Monad<X>.Zero) == monad;
        }

        /// <summary>
        /// Third Monoid Law: Plus is associative.
        /// </summary>
        static bool Monoid_ThirdLaw<X>(Monad<X> a, Monad<X> b, Monad<X> c)
        {
            // mplus a (mplus b c) = mplus (mplus a b) c 
            return a.Plus(b.Plus(c)) == (a.Plus(b)).Plus(c);
        }

        #endregion

        #region Monad Laws

        /// <summary>
        /// First Monad Law: Unit is a left identity for Bind.
        /// </summary>
        static bool Monad_FirstLaw<X, Y>(Kunc<X, Y> f, X value)
        {
            // return x >>= f = f x
            return Monad.Return(value).Bind(f) == f(value);
        }

        /// <summary>
        /// Second Monad Law: Unit is a right identity for Bind.
        /// </summary>
        static bool Monad_SecondLaw<X>(Monad<X> m)
        {
            // m >>= return = m
            return m.Bind(Monad.Return) == m;
        }

        /// <summary>
        /// Third Monad Law: Bind is associative.
        /// </summary>
        static bool Monad_ThirdLaw<X, Y, Z>(Monad<X> m, Kunc<X, Y> f, Kunc<Y, Z> g)
        {
            // (m >>= f) >>= g = m >>= (\x -> f x >>= g)
            return (m.Bind(f)).Bind(g) == m.Bind(_ => f(_).Bind(g));
        }

        //// Same rules but in the Kleisli Category.

        /// <summary>
        /// First Monad Law.
        /// </summary>
        static bool Kleisli_FirstLaw<X, Y>(Kunc<X, Y> g, X value)
        {
            Kunc<X, X> id = _ => Monad.Return(_);

            // return >=> g ≡ g
            return id.Compose(g).Invoke(value) == g(value);
        }

        /// <summary>
        /// Second Monad Law.
        /// </summary>
        static bool Kleisli_SecondLaw<X, Y>(Kunc<X, Y> f, X value)
        {
            // f >=> return ≡ f
            return f.Compose(Monad.Return).Invoke(value) == f(value);
        }

        /// <summary>
        /// Third Monad Law.
        /// </summary>
        static bool Kleisli_ThirdLaw<X, Y, Z, T>(
           Kunc<X, Y> f,
           Kunc<Y, Z> g,
           Kunc<Z, T> h,
           X value)
        {
            // (f >=> g) >=> h ≡ f >=> (g >=> h)
            return (f.Compose(g)).Compose(h).Invoke(value) == f.Compose(g.Compose(h)).Invoke(value);
        }

        #endregion

        /// <summary>
        /// MonadZero: Zero is a left zero for Bind.
        /// </summary>
        static bool MonadZero_LeftZero_Bind<X, Y>(Kunc<X, Y> f, X value)
        {
            // mzero >>= f = mzero
            return Monad<X>.Zero.Bind(f) == Monad<Y>.Zero;
        }

        /// <summary>
        /// MonadMore: Zero is a right zero for Bind.
        /// </summary>
        static bool MonadMore_RightZero_Bind<X>(Monad<X> m)
        {
            // m >>= (\x -> mzero) = mzero
            return m.Bind(_ => Monad<X>.Zero) == Monad<X>.Zero;
        }

        /// <summary>
        /// MonadPlus: Bind is right distributive over Plus.
        /// </summary>
        static bool MonadPlus_RightDistributivity<X>(Monad<X> a, Monad<X> b, Kunc<X, X> f)
        {
            // mplus a b >>= f = mplus (a >>= f) (b >>= f)
            return a.Plus(b).Bind(f) == a.Bind(f).Plus(b.Bind(f));
        }

        /// <summary>
        /// MonadOr: Unit is a left zero for Plus.
        /// </summary>
        static bool MonadOr_LeftZero_Plus<X>(X a, Monad<X> b)
        {
            // morelse (return a) b ≡ return a
            return Monad.Return(a).Plus(b) == Monad.Return(a);
        }

        /// <summary>
        /// Unit is a right zero for Plus.
        /// </summary>
        static bool Unused_UnitIsRightZeroForPlus<X>(Monad<X> a, X b)
        {
            // morelse a (return b) ≡ return b
            return a.Plus(Monad.Return(b)) == Monad.Return(b);
        }
    }
}
