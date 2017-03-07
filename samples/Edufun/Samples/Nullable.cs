// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Samples
{
    using System;

    using Narvalo;

    /// <remarks>
    /// <list type="bullet">
    /// <listheader>What's not to be found here:</listheader>
    /// <item><description><c>Pure</c> is casting: <c>(T?)value</c>.</description></item>
    /// <item><description><c>Nullable</c> does not support the <c>Flatten</c> operation;
    /// there is no <c>Nullable&lt;Nullable&lt;T&gt;&gt;</c>.</description></item>
    /// <item><description><c>Zero</c> is null.</description></item>
    /// </list>
    /// </remarks>
    public static class Nullable
    {
        public static TResult? Bind<TSource, TResult>(this TSource? @this, Func<TSource, TResult?> binder)
            where TSource : struct
            where TResult : struct
        {
            Require.NotNull(binder, nameof(binder));

            return @this.HasValue ? binder(@this.Value) : null;
        }

        #region Query Expression Pattern

        public static TResult? Select<TSource, TResult>(this TSource? @this, Func<TSource, TResult> selector)
            where TSource : struct
            where TResult : struct
        {
            Require.NotNull(selector, nameof(selector));

            return @this.HasValue ? (TResult?)selector(@this.Value) : null;
        }

        public static TSource? Where<TSource>(
            this TSource? @this,
            Func<TSource, bool> predicate)
            where TSource : struct
        {
            Require.NotNull(predicate, nameof(predicate));

            return @this.HasValue && predicate(@this.Value) ? @this : null;
        }

        public static TResult? SelectMany<TSource, TMiddle, TResult>(
            this TSource? @this,
            Func<TSource, TMiddle?> valueSelector,
            Func<TSource, TMiddle, TResult> resultSelector)
            where TSource : struct
            where TMiddle : struct
            where TResult : struct
        {
            Require.NotNull(valueSelector, nameof(valueSelector));
            Require.NotNull(resultSelector, nameof(resultSelector));

            if (!@this.HasValue) { return null; }

            var middle = valueSelector(@this.Value);

            if (!middle.HasValue) { return null; }

            return resultSelector(@this.Value, middle.Value);
        }

        #endregion
    }
}
