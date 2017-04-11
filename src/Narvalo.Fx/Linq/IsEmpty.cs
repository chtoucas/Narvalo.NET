// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;

    public static partial class Qperators
    {
        /// <summary>
        /// Returns true if the sequence is empty; otherwise false.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        public static bool IsEmpty<TSource>(this IEnumerable<TSource> source)
        {
            Require.NotNull(source, nameof(source));

            using (var iter = source.GetEnumerator())
            {
                return !iter.MoveNext();
            }
        }

        /// <summary>
        /// Returns true if no element in the sequence satisfies the predicate;
        /// otherwise false.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        public static bool IsEmpty<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(predicate, nameof(predicate));

            foreach (var element in source)
            {
                if (predicate(element))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
