// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Nullable{T}"/>.
    /// </summary>
    public static class Nullable
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
    }
}
