// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;

    public static partial class Qperators
    {
        // Reduce
        public static TSource Aggregate<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, TSource> accumulator,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(accumulator, nameof(accumulator));
            Require.NotNull(predicate, nameof(predicate));

            using (var iter = @this.GetEnumerator())
            {
                if (!iter.MoveNext())
                {
                    throw new InvalidOperationException("Source sequence was empty.");
                }

                TSource retval = iter.Current;

                while (predicate(retval) && iter.MoveNext())
                {
                    retval = accumulator(retval, iter.Current);
                }

                return retval;
            }
        }

        // Fold
        public static TAccumulate Aggregate<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> accumulator,
            Func<TAccumulate, bool> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(accumulator, nameof(accumulator));
            Require.NotNull(predicate, nameof(predicate));

            TAccumulate retval = seed;

            using (var iter = @this.GetEnumerator())
            {
                while (predicate(retval) && iter.MoveNext())
                {
                    retval = accumulator(retval, iter.Current);
                }
            }

            return retval;
        }

        // Fold
        public static TResult Aggregate<TSource, TAccumulate, TResult>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> accumulator,
            Func<TAccumulate, TResult> resultSelector,
            Func<TAccumulate, bool> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(accumulator, nameof(accumulator));
            Require.NotNull(resultSelector, nameof(resultSelector));
            Require.NotNull(predicate, nameof(predicate));

            TAccumulate retval = seed;

            using (var iter = @this.GetEnumerator())
            {
                while (predicate(retval) && iter.MoveNext())
                {
                    retval = accumulator(retval, iter.Current);
                }
            }

            return resultSelector(retval);
        }
    }
}
