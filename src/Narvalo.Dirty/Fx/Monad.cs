// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    static class Monad
    {
        #region Monads

        static readonly Monad<Unit> Unit_ = Return(Narvalo.Fx.Unit.Single);

        public static Monad<Unit> Unit { get { return Unit_; } }

        public static Monad<T> Return<T>(T value)
        {
            return Monad<T>.η(value);
        }

        public static Monad<T> Join<T>(Monad<Monad<T>> square)
        {
            return Monad<T>.μ(square);
        }

        #endregion

        #region Additive monads

        static readonly Monad<Unit> Zero_ = Monad<Unit>.Zero;

        public static Monad<Unit> Zero { get { return Zero_; } }

        #endregion

        #region Comonads

        public static T Extract<T>(Comonad<T> monad)
        {
            return Comonad<T>.ε(monad);
        }

        public static Comonad<Comonad<T>> Duplicate<T>(Comonad<T> monad)
        {
            return Comonad<T>.δ(monad);
        }

        #endregion

        //// Lift

        public static Func<Monad<T>, Monad<TResult>> Lift<T, TResult>(Func<T, TResult> fun)
        {
            return m => m.Map(fun);
        }

        public static Func<Monad<T1>, Monad<T2>, Monad<TResult>>
            Lift<T1, T2, TResult>(Func<T1, T2, TResult> fun)
        {
            return (m1, m2) => m1.Zip(m2, fun);
        }

        public static Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<TResult>>
            Lift<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> fun)
        {
            return (m1, m2, m3) => m1.Zip(m2, m3, fun);
        }

        public static Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<TResult>>
            Lift<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> fun)
        {
            return (m1, m2, m3, m4) => m1.Zip(m2, m3, m4, fun);
        }

        public static Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<T5>, Monad<TResult>>
            Lift<T1, T2, T3, T4, T5, TResult>(
            Func<T1, T2, T3, T4, T5, TResult> fun)
        {
            return (m1, m2, m3, m4, m5) => m1.Zip(m2, m3, m4, m5, fun);
        }
    }
}
