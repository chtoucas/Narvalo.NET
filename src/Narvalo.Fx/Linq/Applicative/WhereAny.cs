// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq.Applicative
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Narvalo.Applicative;

    public static partial class Aperators
    {
        internal static IEnumerable<TSource> WhereAnyImpl<TSource, TRight>(
            this IEnumerable<TSource> source,
            Func<TSource, Either<bool, TRight>> predicate)
        {
            Debug.Assert(source != null);
            Debug.Assert(predicate != null);

            foreach (var item in source)
            {
                var result = predicate(item);

                if (result != null && result.IsLeft && result.Left) { yield return item; }
            }
        }

        internal static IEnumerable<TSource> WhereAnyImpl<TSource>(
            this IEnumerable<TSource> source,
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

        internal static IEnumerable<TSource> WhereAnyImpl<TSource>(
            this IEnumerable<TSource> source,
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

        internal static IEnumerable<TSource> WhereAnyImpl<TSource>(
            this IEnumerable<TSource> source,
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

        internal static IEnumerable<TSource> WhereAnyImpl<TSource, TError>(
            this IEnumerable<TSource> source,
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
    }
}
