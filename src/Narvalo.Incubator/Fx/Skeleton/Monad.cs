// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Skeleton
{
    static class Monad
    {
        static readonly Monad<Unit> Unit_ = Return(Narvalo.Fx.Unit.Single);
        static readonly Monad<Unit> Zero_ = Monad<Unit>.Zero;

        public static Monad<Unit> Unit { get { return Unit_; } }

        // WARNING: Only for Monads with a Zero.
        public static Monad<Unit> Zero { get { return Zero_; } }

        public static Monad<T> Return<T>(T value)
        {
            return Monad<T>.η(value);
        }

        #region Basic Monad functions

        // [Haskell] >=>
        // Left-to-right Kleisli composition of monads.
        public static Monad<TResult> Compose<TSource, TMiddle, TResult>(
            Kunc<TSource, TMiddle> kunA,
            Kunc<TMiddle, TResult> kunB,
            TSource value)
        {
            Require.NotNull(kunA, "kunA");

            return kunA.Invoke(value).Bind(kunB);
        }

        // [Haskell] <=<
        // Right-to-left Kleisli composition of monads. (>=>), with the arguments flipped.
        public static Monad<TResult> ComposeBack<TSource, TMiddle, TResult>(
            Kunc<TMiddle, TResult> kunA,
            Kunc<TSource, TMiddle> kunB,
            TSource value)
        {
            Require.NotNull(kunB, "kunB");

            return kunB.Invoke(value).Bind(kunA);
        }

        #endregion

        #region Generalisations of list functions

        public static Monad<T> Flatten<T>(Monad<Monad<T>> square)
        {
            return Monad<T>.μ(square);
        }

        #endregion
    }
}
