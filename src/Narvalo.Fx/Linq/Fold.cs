// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;

    public static partial class Qperators
    {
        // Strictly equivalent to Enumerable.Aggregate. Added for symmetry.
        public static TAccumulate Fold<TSource, TAccumulate>(
            this IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(accumulator, nameof(accumulator));

            TAccumulate retval = seed;

            using (var iter = source.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    retval = accumulator(retval, iter.Current);
                }
            }

            return retval;
        }

        public static TAccumulate Fold<TSource, TAccumulate>(
            this IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> accumulator,
            Func<TAccumulate, bool> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(accumulator, nameof(accumulator));
            Require.NotNull(predicate, nameof(predicate));

            TAccumulate retval = seed;

            using (var iter = source.GetEnumerator())
            {
                while (predicate(retval) && iter.MoveNext())
                {
                    retval = accumulator(retval, iter.Current);
                }
            }

            return retval;
        }
    }
}
