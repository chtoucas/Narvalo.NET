// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Narvalo.Applicative;

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

    // Shadowing
    public static partial class Qperators
    {
        internal static IEnumerable<TLeft> SelectAnyImpl<TSource, TLeft, TRight>(
            this IEnumerable<TSource> source,
            Func<TSource, Either<TLeft, TRight>> selector)
        {
            Debug.Assert(source != null);
            Debug.Assert(selector != null);

            foreach (var item in source)
            {
                var result = selector(item);

                if (result != null && result.IsLeft) { yield return result.Left; }
            }
        }

        internal static IEnumerable<TResult> SelectAnyImpl<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, Fallible<TResult>> selector)
        {
            Debug.Assert(source != null);
            Debug.Assert(selector != null);

            foreach (var item in source)
            {
                var result = selector(item);

                if (result.IsSuccess) { yield return result.Value; }
            }
        }

        internal static IEnumerable<TResult> SelectAnyImpl<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, Maybe<TResult>> selector)
        {
            Debug.Assert(source != null);
            Debug.Assert(selector != null);

            foreach (var item in source)
            {
                var result = selector(item);

                if (result.IsSome) { yield return result.Value; }
            }
        }

        internal static IEnumerable<TResult> SelectAnyImpl<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, Outcome<TResult>> selector)
        {
            Debug.Assert(source != null);
            Debug.Assert(selector != null);

            foreach (var item in source)
            {
                var result = selector(item);

                if (result.IsSuccess) { yield return result.Value; }
            }
        }

        internal static IEnumerable<T> SelectAnyImpl<TSource, T, TError>(
            this IEnumerable<TSource> source,
            Func<TSource, Result<T, TError>> selector)
        {
            Debug.Assert(source != null);
            Debug.Assert(selector != null);

            foreach (var item in source)
            {
                var result = selector(item);

                if (result.IsSuccess) { yield return result.Value; }
            }
        }
    }
}
