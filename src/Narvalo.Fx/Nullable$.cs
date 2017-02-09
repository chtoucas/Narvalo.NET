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

        #region Pattern matching

        public static TResult Match<TSource, TResult>(
            this TSource? @this,
            Func<TSource, TResult> caseValue,
            Func<TResult> caseNull)
            where TSource : struct
        {
            Require.NotNull(caseValue, nameof(caseValue));
            Require.NotNull(caseNull, nameof(caseNull));

            return @this.HasValue ? caseValue.Invoke(@this.Value) : caseNull.Invoke();
        }

        public static void Match<TSource>(this TSource? @this, Action<TSource> caseValue, Action caseNull)
            where TSource : struct
        {
            Require.NotNull(caseValue, nameof(caseValue));
            Require.NotNull(caseNull, nameof(caseNull));

            if (@this.HasValue)
            {
                caseValue.Invoke(@this.Value);
            }
            else
            {
                caseNull.Invoke();
            }
        }

        #endregion
    }
}
