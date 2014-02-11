// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Skeleton
{
    using System;

    /*!
     * References:
     * + http://www.haskell.org/onlinereport/monad.html
     */

    static class Prelude
    {
        #region Monad

        // when :: Monad m => Bool -> m () -> m ()
        public static Monad<Unit> When<TSource>(this Monad<TSource> @this, bool predicate, Kunc<Unit, Unit> action)
        {
            Require.NotNull(action, "action");

            return predicate ? action.Invoke(Unit.Single) : Monad.Unit;
        }

        // unless :: Monad m => Bool -> m () -> m ()
        public static Monad<Unit> Unless<TSource>(this Monad<TSource> @this, bool predicate, Kunc<Unit, Unit> action)
        {
            return When(@this, !predicate, action);
        }

        // liftM2 :: Monad m => (a -> b -> c) -> (m a -> m b -> m c)
        public static Monad<TResult> Zip<TFirst, TSecond, TResult>(
            this Monad<TFirst> @this,
            Monad<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second");
            Require.NotNull(resultSelector, "resultSelector");

            return @this.Bind(m1 => second.Map(m2 => resultSelector.Invoke(m1, m2)));
        }

        // liftM3 :: Monad m => (a -> b -> c -> d) -> (m a -> m b -> m c -> m d)
        public static Monad<TResult> Zip<T1, T2, T3, TResult>(
             this Monad<T1> @this,
             Monad<T2> monad2,
             Monad<T3> monad3,
             Func<T1, T2, T3, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(monad2, "monad2");
            Require.NotNull(resultSelector, "resultSelector");

            Kunc<T1, TResult> g
                = t1 => monad2.Zip(monad3, (t2, t3) => resultSelector.Invoke(t1, t2, t3));

            return @this.Bind(g);
        }

        // liftM4 :: Monad m => (a -> b -> c -> d -> e) -> (m a -> m b -> m c -> m d -> m e)
        public static Monad<TResult> Zip<T1, T2, T3, T4, TResult>(
              this Monad<T1> @this,
              Monad<T2> monad2,
              Monad<T3> monad3,
              Monad<T4> monad4,
              Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(monad2, "monad2");
            Require.NotNull(resultSelector, "resultSelector");

            Kunc<T1, TResult> g
                = t1 => monad2.Zip(monad3, monad4, (t2, t3, t4) => resultSelector.Invoke(t1, t2, t3, t4));

            return @this.Bind(g);
        }

        // liftM5 :: Monad m => (a -> b -> c -> d -> e -> f) -> (m a -> m b -> m c -> m d -> m e -> m f)
        public static Monad<TResult> Zip<T1, T2, T3, T4, T5, TResult>(
             this Monad<T1> @this,
             Monad<T2> monad2,
             Monad<T3> monad3,
             Monad<T4> monad4,
             Monad<T5> monad5,
             Func<T1, T2, T3, T4, T5, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(monad2, "monad2");
            Require.NotNull(resultSelector, "resultSelector");

            Kunc<T1, TResult> g
                = t1 => monad2.Zip(monad3, monad4, monad5, (t2, t3, t4, t5) => resultSelector.Invoke(t1, t2, t3, t4, t5));

            return @this.Bind(g);
        }

        #endregion

        #region MonadPlus

        // guard :: :: MonadPlus m => Bool -> m ()
        public static Monad<Unit> Guard<TSource>(this Monad<TSource> @this, bool predicate)
        {
            return predicate ? Monad.Unit : Monad.Zero;
        }

        #endregion
    }
}
