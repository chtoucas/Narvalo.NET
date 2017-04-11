// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq.Applicative
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using Narvalo.Applicative;

    public static partial class Aperators
    {
        internal static IEnumerable<TLeft> CollectAnyImpl<TLeft, TRight>(
            this IEnumerable<Either<TLeft, TRight>> source)
        {
            Debug.Assert(source != null);

            foreach (var item in source)
            {
                if (item != null && item.IsLeft) { yield return item.Left; }
            }
        }

        internal static IEnumerable<TSource> CollectAnyImpl<TSource>(
            this IEnumerable<Fallible<TSource>> source)
        {
            Debug.Assert(source != null);

            foreach (var item in source)
            {
                if (item.IsSuccess) { yield return item.Value; }
            }
        }

        internal static IEnumerable<TSource> CollectAnyImpl<TSource>(
            this IEnumerable<Maybe<TSource>> source)
        {
            Debug.Assert(source != null);

            foreach (var item in source)
            {
                if (item.IsSome) { yield return item.Value; }
            }
        }

        internal static IEnumerable<TSource> CollectAnyImpl<TSource>(
            this IEnumerable<Outcome<TSource>> source)
        {
            Debug.Assert(source != null);

            foreach (var item in source)
            {
                if (item.IsSuccess) { yield return item.Value; }
            }
        }

        internal static IEnumerable<TSource> CollectAnyImpl<TSource, TError>(
            this IEnumerable<Result<TSource, TError>> source)
        {
            Debug.Assert(source != null);

            foreach (var item in source)
            {
                if (item.IsSuccess) { yield return item.Value; }
            }
        }
    }
}
