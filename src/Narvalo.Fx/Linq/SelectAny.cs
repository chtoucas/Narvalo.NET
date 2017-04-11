// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;

    public static partial class Qperators
    {
        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TResult> selector)
            where TResult : class
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(selector, nameof(selector));

            return iterator();

            IEnumerable<TResult> iterator()
            {
                foreach (var item in source)
                {
                    var result = selector(item);

                    if (result != null) { yield return result; }
                }
            }
        }

        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TResult?> selector)
            where TResult : struct
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(selector, nameof(selector));

            return iterator();

            IEnumerable<TResult> iterator()
            {
                foreach (var item in source)
                {
                    var result = selector(item);

                    if (result.HasValue) { yield return result.Value; }
                }
            }
        }
    }
}
