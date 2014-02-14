// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Skeleton
{
    using System;
    using System.Collections.Generic;

    static partial class MonadExtensions
    {
        #region Restriction Operators

        // If exists, it is used by Query Expression Pattern.
        // WARNING: Only for Monads with a Zero.
        public static Monad<TSource> Where<TSource>(this Monad<TSource> @this, Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");

            // Or simply: @this.Then(predicate, @this);
            return @this.Bind(_ => predicate.Invoke(_) ? @this : Monad<TSource>.Zero);
        }

        #endregion

        #region Projection Operators

        // This is just Map / Zip, or liftM in Haskell.
        // If exists, it is used by Query Expression Pattern.
        public static Monad<TResult> Select<TSource, TResult>(this Monad<TSource> @this, Func<TSource, TResult> selector)
        {
            Require.Object(@this);

            return @this.Map(selector);
        }

        // Alias for Bind.
        // WARNING: Private since it won't be implemented for a concrete Monad (not used by the Query Expression Pattern).
        static Monad<TResult> SelectMany<TSource, TResult>(
           this Monad<TSource> @this,
           Kunc<TSource, TResult> selector)
        {
            Require.Object(@this);

            return @this.Bind(selector);
        }

        // Generalisation of Zip, or liftM2 in Haskell.
        // If exists, it is used by Query Expression Pattern.
        public static Monad<TResult> SelectMany<TSource, TMiddle, TResult>(
            this Monad<TSource> @this,
            Func<TSource, Monad<TMiddle>> valueSelector,
            Func<TSource, TMiddle, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(valueSelector, "valueSelector");
            Require.NotNull(resultSelector, "resultSelector");

            return @this.Bind(_ => valueSelector.Invoke(_).Map(middle => resultSelector.Invoke(_, middle)));
        }

        #endregion

        #region Join Operators

        // If exists, it is used by Query Expression Pattern.
        public static Monad<TResult> Join<TSource, TInner, TKey, TResult>(
            this Monad<TSource> @this,
            Monad<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector)
        {
            return Join(@this, inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default);
        }

        // If exists, it is used by Query Expression Pattern.
        public static Monad<TResult> Join<TSource, TInner, TKey, TResult>(
            this Monad<TSource> @this,
            Monad<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            Require.Object(@this);
            Require.NotNull(inner, "inner");
            Require.NotNull(outerKeySelector, "valueSelector");
            Require.NotNull(innerKeySelector, "innerKeySelector");
            Require.NotNull(resultSelector, "resultSelector");

            throw new NotImplementedException();
        }

        #endregion
    }
}
