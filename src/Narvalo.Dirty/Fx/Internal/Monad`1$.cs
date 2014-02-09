// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Internal
{
    using System;

    static partial class MonadExtensions
    {
        //// Zip

        public static Monad<TResult> Zip<TFirst, TSecond, TResult>(
            this Monad<TFirst> @this,
            Monad<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            return @this.Bind(firstValue => second.Map(secondValue => resultSelector.Invoke(firstValue, secondValue)));
        }

        //// Run

        public static Monad<TSource> Run<TSource>(this Monad<TSource> @this, Action<TSource> action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            return @this.Bind(action.ToKunc()).Then(@this);
        }

        #region Only for monads with Zero

        //// Coalescing

        public static Monad<TResult> Coalesce<TSource, TResult>(
            this Monad<TSource> @this,
            Monad<TResult> then,
            Monad<TResult> otherwise)
        {
            Require.Object(@this);

            return @this.Then(then).Otherwise(otherwise);
        }

        public static Monad<TResult> Then<TSource, TResult>(this Monad<TSource> @this, Monad<TResult> other)
        {
            Require.Object(@this);

            return @this.Bind(_ => other);
        }

        public static Monad<TResult> Otherwise<TSource, TResult>(this Monad<TSource> @this, Monad<TResult> other)
        {
            Require.Object(@this);

            return @this.Bind(_ => Monad.Zero).Bind(_ => other);
        }

        //// OnZero

        public static Monad<TSource> OnZero<TSource>(this Monad<TSource> @this, Action action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            return @this.Otherwise(Monad.Unit).Bind(action.ToKunc()).Then(@this);
        }

        #endregion

        #region Linq

        //// Restriction Operators

        public static Monad<TSource> Where<TSource>(this Monad<TSource> @this, Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");

            return @this.Map(predicate).Then(@this);
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
