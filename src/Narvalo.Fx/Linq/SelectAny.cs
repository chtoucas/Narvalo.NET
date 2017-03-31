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
            this IEnumerable<TSource> @this,
            Func<TSource, TResult?> selector)
            where TResult : struct
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));

            return SelectAnyIterator(@this, selector);
        }

        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, TResult> selector)
            where TResult : class
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));

            return SelectAnyIterator(@this, selector);
        }

        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<TResult>> selector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));

            return SelectAnyIterator(@this, selector);
        }

        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Outcome<TResult>> selector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));

            return SelectAnyIterator(@this, selector);
        }

        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Fallible<TResult>> selector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));

            return SelectAnyIterator(@this, selector);
        }

        public static IEnumerable<T> SelectAny<TSource, T, TError>(
            this IEnumerable<TSource> @this,
            Func<TSource, Result<T, TError>> selector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));

            return SelectAnyIterator(@this, selector);
        }

        public static IEnumerable<TLeft> SelectAny<TSource, TLeft, TRight>(
            this IEnumerable<TSource> @this,
            Func<TSource, Either<TLeft, TRight>> selector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));

            return SelectAnyIterator(@this, selector);
        }

        private static IEnumerable<TResult> SelectAnyIterator<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, TResult?> selector)
            where TResult : struct
        {
            Debug.Assert(source != null);
            Debug.Assert(selector != null);

            foreach (var item in source)
            {
                var m = selector(item);

                if (m.HasValue) { yield return m.Value; }
            }
        }

        private static IEnumerable<TResult> SelectAnyIterator<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, TResult> selector)
            where TResult : class
        {
            Debug.Assert(source != null);
            Debug.Assert(selector != null);

            foreach (var item in source)
            {
                var m = selector(item);

                if (m != null) { yield return m; }
            }
        }

        private static IEnumerable<TResult> SelectAnyIterator<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, Maybe<TResult>> selector)
        {
            Debug.Assert(source != null);
            Debug.Assert(selector != null);

            foreach (var item in source)
            {
                var m = selector(item);

                if (m.IsSome) { yield return m.Value; }
            }
        }

        private static IEnumerable<TResult> SelectAnyIterator<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, Outcome<TResult>> selector)
        {
            Debug.Assert(source != null);
            Debug.Assert(selector != null);

            foreach (var item in source)
            {
                var m = selector(item);

                if (m.IsSuccess) { yield return m.Value; }
            }
        }

        private static IEnumerable<TResult> SelectAnyIterator<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, Fallible<TResult>> selector)
        {
            Debug.Assert(source != null);
            Debug.Assert(selector != null);

            foreach (var item in source)
            {
                var m = selector(item);

                if (m.IsSuccess) { yield return m.Value; }
            }
        }

        private static IEnumerable<T> SelectAnyIterator<TSource, T, TError>(
            IEnumerable<TSource> source,
            Func<TSource, Result<T, TError>> selector)
        {
            Debug.Assert(source != null);
            Debug.Assert(selector != null);

            foreach (var item in source)
            {
                var m = selector(item);

                if (m.IsSuccess) { yield return m.Value; }
            }
        }

        private static IEnumerable<TLeft> SelectAnyIterator<TSource, TLeft, TRight>(
            IEnumerable<TSource> source,
            Func<TSource, Either<TLeft, TRight>> selector)
        {
            Debug.Assert(source != null);
            Debug.Assert(selector != null);

            foreach (var item in source)
            {
                var m = selector(item);

                if (m.IsLeft) { yield return m.Left; }
            }
        }
    }
}
