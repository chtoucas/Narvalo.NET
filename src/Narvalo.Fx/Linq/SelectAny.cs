// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Applicative;

    public static partial class Sequence
    {
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
            Func<TSource, Maybe<TResult>> selector)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(selector, nameof(selector));

            return iterator();

            IEnumerable<TResult> iterator()
            {
                foreach (var item in source)
                {
                    var result = selector(item);

                    if (result.IsSome) { yield return result.Value; }
                }
            }
        }

        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, Outcome<TResult>> selector)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(selector, nameof(selector));

            return iterator();

            IEnumerable<TResult> iterator()
            {
                foreach (var item in source)
                {
                    var result = selector(item);

                    if (result.IsSuccess) { yield return result.Value; }
                }
            }
        }

        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, Fallible<TResult>> selector)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(selector, nameof(selector));

            return iterator();

            IEnumerable<TResult> iterator()
            {
                foreach (var item in source)
                {
                    var result = selector(item);

                    if (result.IsSuccess) { yield return result.Value; }
                }
            }
        }

        public static IEnumerable<T> SelectAny<TSource, T, TError>(
            this IEnumerable<TSource> source,
            Func<TSource, Result<T, TError>> selector)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(selector, nameof(selector));

            return iterator();

            IEnumerable<T> iterator()
            {
                foreach (var item in source)
                {
                    var result = selector(item);

                    if (result.IsSuccess) { yield return result.Value; }
                }
            }
        }

        public static IEnumerable<TLeft> SelectAny<TSource, TLeft, TRight>(
            this IEnumerable<TSource> source,
            Func<TSource, Either<TLeft, TRight>> selector)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(selector, nameof(selector));

            return iterator();

            IEnumerable<TLeft> iterator()
            {
                foreach (var item in source)
                {
                    var result = selector(item);

                    if (result.IsLeft) { yield return result.Left; }
                }
            }
        }
    }
}
