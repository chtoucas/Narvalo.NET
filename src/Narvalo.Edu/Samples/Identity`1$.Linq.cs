// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Samples
{
    using System;

    /// <summary>
    /// Provides limited support for the Query Expression Pattern with <see cref="Narvalo.Fx.Identity&lt;T&gt;"/>.
    /// </summary>
    static partial class IdentityExtensions
    {
        #region Projection Operators

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

        #endregion
    }
}
