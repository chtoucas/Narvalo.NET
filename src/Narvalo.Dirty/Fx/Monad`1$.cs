// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    static partial class MonadExtensions
    {
        #region Monad extensions

        //// Zip

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

        //// Run

        public static Monad<TSource> Run<TSource>(this Monad<TSource> @this, Action<TSource> action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            @this.Bind(action.ToKunc());

            return @this;
        }

        #endregion

        #region Optional monad extensions: higher forms of Zip (LiftM3...5)

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

        #region Additive monad extensions

        //// Then

        public static Monad<TResult> Then<TSource, TResult>(this Monad<TSource> @this, Monad<TResult> other)
        {
            Require.Object(@this);

            // @this == Zero => Zero
            // @this != Zero => other

            return @this.Bind(_ => other);
        }

        //// Otherwise

        public static Monad<TResult> Otherwise<TSource, TResult>(this Monad<TSource> @this, Monad<TResult> other)
        {
            Require.Object(@this);

            // @this == Zero => other
            // @this != Zero => Zero

            var x = @this.Bind(_ => Monad.Zero);

            return @this.Then(Monad.Zero).Then(other);
        }

        //// Coalesce

        // FIXME
        public static Monad<TResult> Coalesce<TSource, TResult>(
            this Monad<TSource> @this,
            Monad<TResult> then,
            Monad<TResult> otherwise)
        {
            Require.Object(@this);

            return @this.Then(then).Otherwise(otherwise);
        }

        //// OnZero

        // FIXME
        public static Monad<TSource> OnZero<TSource>(this Monad<TSource> @this, Action action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            @this.Otherwise(Monad.Unit).Bind(action.ToKunc());

             return @this;
        }

        #endregion

        #region Linq extensions

        //// Restriction Operators

        // FIXME
        public static Monad<TSource> Where<TSource>(this Monad<TSource> @this, Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");

            return @this.Map(predicate).Bind(_ => @this);
        }

        //// Projection Operators

        public static Monad<TResult> Select<TSource, TResult>(this Monad<TSource> @this, Func<TSource, TResult> selector)
        {
            Require.Object(@this);

            return @this.Map(selector);
        }

        public static Monad<TResult> SelectMany<TSource, TMiddle, TResult>(
            this Monad<TSource> @this,
            Func<TSource, Monad<TMiddle>> valueSelector,
            Func<TSource, TMiddle, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(valueSelector, "valueSelector");
            Require.NotNull(resultSelector, "resultSelector");

            return @this.Bind(_ => valueSelector(_).Map(m => resultSelector(_, m)));
        }

        #endregion
    }
}
