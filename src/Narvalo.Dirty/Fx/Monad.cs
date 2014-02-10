// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    static class Monad
    {
        static readonly Monad<Unit> Zero_ = Monad<Unit>.Zero;
        static readonly Monad<Unit> Unit_ = Return(Narvalo.Fx.Unit.Single);

        public static Monad<Unit> Zero { get { return Zero_; } }

        // WARNING: À ne pas confondre avec l'unité (Return).
        public static Monad<Unit> Unit { get { return Unit_; } }

        //// Return (Unit). Personaly, I prefer Create.

        public static Monad<T> Return<T>(T value)
        {
            return Monad<T>.η(value);
        }

        //// Extract (Counit, Coreturn)

        public static T Extract<T>(Monad<T> monad)
        {
            return Monad<T>.ε(monad);
        }

        //// Join

        public static Monad<T> Join<T>(Monad<Monad<T>> square)
        {
            return Monad<T>.μ(square);
        }

        //// Duplicate

        public static Monad<Monad<T>> Duplicate<T>(Monad<T> monad)
        {
            return Monad<T>.δ(monad);
        }

        //// Lift

        [Obsolete]
        public static Func<Monad<T>, Monad<TResult>> Lift<T, TResult>(Func<T, TResult> fun)
        {
            return _ => _.Map(fun);
        }

        [Obsolete]
        public static Func<Monad<T1>, Monad<T2>, Monad<TResult>>
            Lift<T1, T2, TResult>(Func<T1, T2, TResult> fun)
        {
            return (m1, m2) => m1.Zip(m2, fun);
        }

        [Obsolete]
        public static Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<TResult>>
            Lift<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> fun)
        {
            return (m1, m2, m3) => m1.Zip(m2, m3, fun);
        }

        [Obsolete]
        public static Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<TResult>>
            Lift<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> fun)
        {
            return (m1, m2, m3, m4) => m1.Zip(m2, m3, m4, fun);
        }

        [Obsolete]
        public static Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<T5>, Monad<TResult>>
            Lift<T1, T2, T3, T4, T5, TResult>(
            Func<T1, T2, T3, T4, T5, TResult> fun)
        {
            return (m1, m2, m3, m4, m5) => m1.Zip(m2, m3, m4, m5, fun);
        }
    }
}
