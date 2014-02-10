// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using Narvalo.Fx;

    /// <summary>
    /// Provides limited support for the Query Expression Pattern with <see cref="Narvalo.Fx.Output&lt;T&gt;"/>.
    /// </summary>
    public static class OutputExtensions
    {
        //// Projection Operators

        public static Output<TResult> Select<TSource, TResult>(
            this Output<TSource> @this,
            Func<TSource, TResult> selector)
        {
            Require.Object(@this);

            return @this.Map(selector);
        }

        public static Output<TResult> SelectMany<TSource, TMiddle, TResult>(
            this Output<TSource> @this,
            Func<TSource, Output<TMiddle>> valueSelector,
            Func<TSource, TMiddle, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(valueSelector, "valueSelector");
            Require.NotNull(resultSelector, "resultSelector");

            return @this.Bind(_ => valueSelector(_).Map(m => resultSelector(_, m)));
        }
    }
}
