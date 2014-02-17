// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public static class Monad
    {
        static readonly Monad<Unit> Unit_ = Return(Narvalo.Edu.Fx.Unit.Single);
#if !MONAD_DISABLE_ZERO
        static readonly Monad<Unit> Zero_ = Monad<Unit>.Zero;
#endif

        public static Monad<Unit> Unit { get { return Unit_; } }

#if !MONAD_DISABLE_ZERO
        // [Haskell] mzero
        public static Monad<Unit> Zero { get { return Zero_; } }
#endif

        // [Haskell] return
        public static Monad<T> Return<T>(T value)
        {
            return Monad<T>.η(value);
        }

        #region Generalisations of list functions

        // [Haskell] join
        public static Monad<T> Flatten<T>(Monad<Monad<T>> square)
        {
            return Monad<T>.μ(square);
        }

        #endregion

        #region Monadic lifting operators

        public static Func<Monad<T>, Monad<TResult>> Lift<T, TResult>(Func<T, TResult> fun)
        {
            return m => m.Select(fun);
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

        #endregion

        #region Conditional execution of monadic expressions

#if !MONAD_DISABLE_ZERO
        // [Haskell] guard
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static Monad<Unit> Guard<TSource>(bool predicate)
        {
            return predicate ? Unit : Zero;
        }
#endif

        // [Haskell] when
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static Monad<Unit> When<TSource>(bool predicate, Kunc<Unit, Unit> action)
        {
            Require.NotNull(action, "action");

            return predicate ? action.Invoke(Narvalo.Edu.Fx.Unit.Single) : Unit;
        }

        // [Haskell] unless
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static Monad<Unit> Unless<TSource>(bool predicate, Kunc<Unit, Unit> action)
        {
            return When<TSource>(!predicate, action);
        }

        #endregion
    }
}
