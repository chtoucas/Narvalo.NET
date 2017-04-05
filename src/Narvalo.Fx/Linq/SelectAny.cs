// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Applicative;

    public static partial class Qperators
    {
        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TResult?> selector)
            where TResult : struct
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(selector, nameof(selector));

            return Iterator();

            IEnumerable<TResult> Iterator()
            {
                foreach (var item in source)
                {
                    var m = selector(item);

                    if (m.HasValue) { yield return m.Value; }
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

            return Iterator();

            IEnumerable<TResult> Iterator()
            {
                foreach (var item in source)
                {
                    var m = selector(item);

                    if (m != null) { yield return m; }
                }
            }
        }

        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, Maybe<TResult>> selector)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(selector, nameof(selector));

            return Iterator();

            IEnumerable<TResult> Iterator()
            {
                foreach (var item in source)
                {
                    var m = selector(item);

                    if (m.IsSome) { yield return m.Value; }
                }
            }
        }

        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, Outcome<TResult>> selector)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(selector, nameof(selector));

            return Iterator();

            IEnumerable<TResult> Iterator()
            {
                foreach (var item in source)
                {
                    var m = selector(item);

                    if (m.IsSuccess) { yield return m.Value; }
                }
            }
        }

        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, Fallible<TResult>> selector)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(selector, nameof(selector));

            return Iterator();

            IEnumerable<TResult> Iterator()
            {
                foreach (var item in source)
                {
                    var m = selector(item);

                    if (m.IsSuccess) { yield return m.Value; }
                }
            }
        }

        public static IEnumerable<T> SelectAny<TSource, T, TError>(
            this IEnumerable<TSource> source,
            Func<TSource, Result<T, TError>> selector)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(selector, nameof(selector));

            return Iterator();

            IEnumerable<T> Iterator()
            {
                foreach (var item in source)
                {
                    var m = selector(item);

                    if (m.IsSuccess) { yield return m.Value; }
                }
            }
        }

        public static IEnumerable<TLeft> SelectAny<TSource, TLeft, TRight>(
            this IEnumerable<TSource> source,
            Func<TSource, Either<TLeft, TRight>> selector)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(selector, nameof(selector));

            return Iterator();

            IEnumerable<TLeft> Iterator()
            {
                foreach (var item in source)
                {
                    var m = selector(item);

                    if (m.IsLeft) { yield return m.Left; }
                }
            }
        }
    }
}
