// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    static class MonadExtensions
    {
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

        //// Run

        public static Monad<Unit> Run<TSource>(this Monad<TSource> @this, Kunc<TSource, Unit> action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            return @this.Bind(action);
        }

        public static Monad<TSource> Run<TSource>(this Monad<TSource> @this, Action<TSource> action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            @this.Bind(action.ToKunc());

            return @this;
        }

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

        public static Monad<Unit> OnZero<TSource>(this Monad<TSource> @this, Kunc<Unit, Unit> action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            return @this.Otherwise(Monad.Unit).Bind(action);
        }

        public static Monad<TSource> OnZero<TSource>(this Monad<TSource> @this, Action action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            @this.Otherwise(Monad.Unit).Bind(action.ToKunc());

            return @this;
        }
    }
}
