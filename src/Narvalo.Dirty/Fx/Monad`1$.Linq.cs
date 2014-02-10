// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    static partial class MonadExtensions
    {
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
    }
}
