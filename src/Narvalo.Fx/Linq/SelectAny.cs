// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;

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

        // Named <c>mapMaybe</c> in Haskell parlance.
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
            Func<TSource, Result<TResult>> selector)
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
            Demand.NotNull(source);
            Demand.NotNull(selector);

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
            Demand.NotNull(source);
            Demand.NotNull(selector);

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
            Demand.NotNull(source);
            Demand.NotNull(selector);

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
            Demand.NotNull(source);
            Demand.NotNull(selector);

            foreach (var item in source)
            {
                var m = selector.Invoke(item);

                if (m.IsSuccess) { yield return m.Value; }
            }
        }

        private static IEnumerable<TResult> SelectAnyIterator<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, Result<TResult>> selector)
        {
            Demand.NotNull(source);
            Demand.NotNull(selector);

            foreach (var item in source)
            {
                var m = selector.Invoke(item);

                if (m.IsSuccess) { yield return m.Value; }
            }
        }
    }
}
