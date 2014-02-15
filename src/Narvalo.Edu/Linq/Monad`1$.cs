// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Linq
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Edu.Fx;

    /*!
     * Where and Select are already part of the Monad definition.
     */

    static class MonadExtensions
    {
        //[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
        //    Justification = "This is just Bind and it is not part of the Query Expression Pattern")]
        //static Monad<TResult> SelectMany<TSource, TResult>(
        //   this Monad<TSource> @this,
        //   Kunc<TSource, TResult> selector)
        //{
        //    Require.Object(@this);

        //    return @this.Bind(selector);
        //}

        // Generalisation of Zip, or liftM2 in Haskell.
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

        public static Monad<TResult> Join<TSource, TInner, TKey, TResult>(
            this Monad<TSource> @this,
            Monad<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector)
        {
            return Join(@this, inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default);
        }

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

        public static Monad<TResult> GroupJoin<TSource, TInner, TKey, TResult>(
            this Monad<TSource> @this,
            Monad<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, Monad<TInner>, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(inner, "inner");
            Require.NotNull(outerKeySelector, "valueSelector");
            Require.NotNull(innerKeySelector, "innerKeySelector");
            Require.NotNull(resultSelector, "resultSelector");

            throw new NotImplementedException();
        }
    }
}
