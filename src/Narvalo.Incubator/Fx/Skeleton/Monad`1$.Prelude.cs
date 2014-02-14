// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Skeleton
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    static partial class MonadExtensions
    {
        #region Monadic lifting operators

        // [Haskell] liftM
        // Promote a function to a monad.
        // WARNING: Private since it won't be implemented for a concrete Monad (this is just Map).
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Monad template definition.")]
        static Monad<TResult> Zip<TFirst, TResult>(
           this Monad<TFirst> @this,
           Func<TFirst, TResult> resultSelector)
        {
            Require.Object(@this);

            return @this.Map(resultSelector);
        }

        // [Haskell] liftM2
        // Promote a function to a monad, scanning the monadic arguments from left to right.
        public static Monad<TResult> Zip<TFirst, TSecond, TResult>(
            this Monad<TFirst> @this,
            Monad<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second");
            Require.NotNull(resultSelector, "resultSelector");

            return @this.Bind(_ => second.Map(v2 => resultSelector.Invoke(_, v2)));
        }

        // [Haskell] liftM3
        // Promote a function to a monad, scanning the monadic arguments from left to right.
        public static Monad<TResult> Zip<T1, T2, T3, TResult>(
             this Monad<T1> @this,
             Monad<T2> second,
             Monad<T3> third,
             Func<T1, T2, T3, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second");
            Require.NotNull(resultSelector, "resultSelector");

            Kunc<T1, TResult> g
                = t1 => second.Zip(third, (t2, t3) => resultSelector.Invoke(t1, t2, t3));

            return @this.Bind(g);
        }

        // [Haskell] liftM4
        // Promote a function to a monad, scanning the monadic arguments from left to right.
        public static Monad<TResult> Zip<T1, T2, T3, T4, TResult>(
              this Monad<T1> @this,
              Monad<T2> second,
              Monad<T3> third,
              Monad<T4> fourth,
              Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second");
            Require.NotNull(resultSelector, "resultSelector");

            Kunc<T1, TResult> g
                = t1 => second.Zip(third, fourth, (t2, t3, t4) => resultSelector.Invoke(t1, t2, t3, t4));

            return @this.Bind(g);
        }

        // [Haskell] liftM5
        // Promote a function to a monad, scanning the monadic arguments from left to right.
        public static Monad<TResult> Zip<T1, T2, T3, T4, T5, TResult>(
             this Monad<T1> @this,
             Monad<T2> second,
             Monad<T3> third,
             Monad<T4> fourth,
             Monad<T5> fifth,
             Func<T1, T2, T3, T4, T5, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second");
            Require.NotNull(resultSelector, "resultSelector");

            Kunc<T1, TResult> g
                = t1 => second.Zip(third, fourth, fifth, (t2, t3, t4, t5) => resultSelector.Invoke(t1, t2, t3, t4, t5));

            return @this.Bind(g);
        }

        #endregion
    }
}
