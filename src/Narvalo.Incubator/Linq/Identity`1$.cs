// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using Narvalo.Fx;

    /// <summary>
    /// Provides limited support for the Query Expression Pattern with <see cref="Narvalo.Fx.Identity&lt;T&gt;"/>.
    /// </summary>
    public static class NullableExtensions
    {
       //// Restriction Operators

        public static Identity<TSource> Where<TSource>(
            this Identity<TSource> @this,
            Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");

            return @this.Map(predicate).Then(@this);
        }

        //// Projection Operators

        public static Identity<TResult> Select<TSource, TResult>(
            this Identity<TSource> @this,
            Func<TSource, TResult> selector)
        {
            Require.Object(@this);

            return @this.Map(selector);
        }

        public static Identity<TResult> SelectMany<TSource, TMiddle, TResult>(
            this Identity<TSource> @this,
            Func<TSource, Identity<TMiddle>> valueSelector,
            Func<TSource, TMiddle, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(valueSelector, "valueSelector");
            Require.NotNull(resultSelector, "resultSelector");

            return @this.Bind(_ => valueSelector(_).Map(m => resultSelector(_, m)));
        }
    }
}
