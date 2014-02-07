// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    /// <summary>
    /// Provides extension methods for <see cref="System.Nullable{T}"/>.
    /// </summary>
    public static partial class NullableExtensions
    {
        //// Match

        public static TResult Match<TSource, TResult>(
            this TSource? @this,
            Func<TSource, TResult> selector,
            TResult defaultValue)
            where TSource : struct
            where TResult : struct
        {
            return @this.Map(selector) ?? defaultValue;
        }

        public static TResult Match<TSource, TResult>(
            this TSource? @this,
            Func<TSource, TResult> selector,
            Func<TResult> defaultValueFactory)
            where TSource : struct
            where TResult : struct
        {
            Require.NotNull(defaultValueFactory, "defaultValueFactory");

            return @this.Match(selector, defaultValueFactory.Invoke());
        }

        //// Then

        public static TResult? Then<TSource, TResult>(this TSource? @this, TResult? other)
            where TSource : struct
            where TResult : struct
        {
            return @this.Bind(_ => other);
        }
    }
}
