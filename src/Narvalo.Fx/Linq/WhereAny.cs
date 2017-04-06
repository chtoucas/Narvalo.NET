// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Narvalo.Applicative;

    public static partial class Qperators
    {
        public static IEnumerable<TSource> WhereAny<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool?> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(predicate, nameof(predicate));

            return WhereAnyIterator(source, predicate);
        }

        public static IEnumerable<TSource> WhereAny<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, Maybe<bool>> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(predicate, nameof(predicate));

            return WhereAnyIterator(source, predicate);
        }

        public static IEnumerable<TSource> WhereAny<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, Outcome<bool>> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(predicate, nameof(predicate));

            return WhereAnyIterator(source, predicate);
        }

        public static IEnumerable<TSource> WhereAny<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, Fallible<bool>> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(predicate, nameof(predicate));

            return WhereAnyIterator(source, predicate);
        }

        public static IEnumerable<TSource> WhereAny<TSource, TError>(
            this IEnumerable<TSource> source,
            Func<TSource, Result<bool, TError>> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(predicate, nameof(predicate));

            return WhereAnyIterator(source, predicate);
        }

        public static IEnumerable<TSource> WhereAny<TSource, TRight>(
            this IEnumerable<TSource> source,
            Func<TSource, Either<bool, TRight>> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(predicate, nameof(predicate));

            return WhereAnyIterator(source, predicate);
        }

        private static IEnumerable<TSource> WhereAnyIterator<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, bool?> predicate)
        {
            Debug.Assert(source != null);
            Debug.Assert(predicate != null);

            foreach (var item in source)
            {
                var result = predicate(item);

                if (result.HasValue && result.Value) { yield return item; }
            }
        }

        private static IEnumerable<TSource> WhereAnyIterator<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, Maybe<bool>> predicate)
        {
            Debug.Assert(source != null);
            Debug.Assert(predicate != null);

            foreach (var item in source)
            {
                var result = predicate(item);

                if (result.IsSome && result.Value) { yield return item; }
            }
        }

        private static IEnumerable<TSource> WhereAnyIterator<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, Outcome<bool>> predicate)
        {
            Debug.Assert(source != null);
            Debug.Assert(predicate != null);

            foreach (var item in source)
            {
                var result = predicate(item);

                if (result.IsSuccess && result.Value) { yield return item; }
            }
        }

        private static IEnumerable<TSource> WhereAnyIterator<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, Fallible<bool>> predicate)
        {
            Debug.Assert(source != null);
            Debug.Assert(predicate != null);

            foreach (var item in source)
            {
                var result = predicate(item);

                if (result.IsSuccess && result.Value) { yield return item; }
            }
        }

        private static IEnumerable<TSource> WhereAnyIterator<TSource, TError>(
            IEnumerable<TSource> source,
            Func<TSource, Result<bool, TError>> predicate)
        {
            Debug.Assert(source != null);
            Debug.Assert(predicate != null);

            foreach (var item in source)
            {
                var result = predicate(item);

                if (result.IsSuccess && result.Value) { yield return item; }
            }
        }

        private static IEnumerable<TSource> WhereAnyIterator<TSource, TRight>(
            IEnumerable<TSource> source,
            Func<TSource, Either<bool, TRight>> predicate)
        {
            Debug.Assert(source != null);
            Debug.Assert(predicate != null);

            foreach (var item in source)
            {
                var result = predicate(item);

                if (result.IsLeft && result.Left) { yield return item; }
            }
        }
    }
}
