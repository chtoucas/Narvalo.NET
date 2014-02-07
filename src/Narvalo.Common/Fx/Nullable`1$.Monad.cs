// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public static partial class NullableExtensions
    {
        public static TResult? Bind<TSource, TResult>(this TSource? @this, Func<TSource, TResult?> selector)
            where TSource : struct
            where TResult : struct
        {
            Require.NotNull(selector, "selector");

            return @this.HasValue ? selector.Invoke(@this.Value) : null;
        }

        public static TResult? Map<TSource, TResult>(this TSource? @this, Func<TSource, TResult> selector)
            where TSource : struct
            where TResult : struct
        {
            Require.NotNull(selector, "selector");

            return @this.HasValue ? (TResult?)selector.Invoke(@this.Value) : null;
        }
    }
}
