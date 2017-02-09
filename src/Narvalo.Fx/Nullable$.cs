// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    /// <summary>
    /// Provides extension methods for <see cref="Nullable{T}"/>.
    /// </summary>
    public static class NullableExtensions
    {
        #region Basic Monad functions

        public static TResult? Select<TSource, TResult>(this TSource? @this, Func<TSource, TResult> selector)
            where TSource : struct
            where TResult : struct
        {
            Require.NotNull(selector, nameof(selector));

            return @this.HasValue ? (TResult?)selector.Invoke(@this.Value) : null;
        }

        #endregion

        //public static TResult Project<TSource, TResult>(
        //    this TSource? @this,
        //    Func<TSource, TResult> selector,
        //    TResult caseNull)
        //    where TSource : struct
        //{
        //    Require.NotNull(selector, nameof(selector));

        //    return @this.HasValue ? selector.Invoke(@this.Value) : caseNull;
        //}

        //public static TResult Project<TSource, TResult>(
        //    this TSource? @this,
        //    Func<TSource, TResult> selector,
        //    Func<TResult> caseNull)
        //    where TSource : struct
        //{
        //    Require.NotNull(selector, nameof(selector));
        //    Require.NotNull(caseNull, nameof(caseNull));

        //    return @this.HasValue ? selector.Invoke(@this.Value) : caseNull.Invoke();
        //}
    }
}
