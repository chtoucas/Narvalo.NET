// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    using Narvalo;

    // Query Expression Pattern for nullables.
    public static class Qullable
    {
        public static TResult? Select<TSource, TResult>(this TSource? @this, Func<TSource, TResult> selector)
            where TSource : struct
            where TResult : struct
        {
            Require.NotNull(selector, nameof(selector));

            return @this is TSource v ? (TResult?)selector(v) : null;
        }

        public static TSource? Where<TSource>(
            this TSource? @this,
            Func<TSource, bool> predicate)
            where TSource : struct
        {
            Require.NotNull(predicate, nameof(predicate));

            return @this is TSource v && predicate(v) ? @this : null;
        }

        public static TResult? SelectMany<TSource, TResult>(
            this TSource? @this,
            Func<TSource, TResult?> selector)
            where TSource : struct
            where TResult : struct
        {
            Require.NotNull(selector, nameof(selector));

            return @this is TSource v ? selector(v) : null;
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

            return @this is TSource v && valueSelector(v) is TMiddle m
                ? resultSelector(v, m)
                : (TResult?)null;
        }
    }
}
