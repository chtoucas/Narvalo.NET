// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;

    public static partial class Qperators
    {
        /// <summary>
        /// Filters a sequence of values based on a predicate while discarding
        /// any value for which the filter returns null.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence to filter.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        public static IEnumerable<TSource> WhereAny<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool?> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(predicate, nameof(predicate));

            return iterator();

            IEnumerable<TSource> iterator()
            {
                foreach (var item in source)
                {
                    var result = predicate(item);

                    if (result.HasValue && result.Value) { yield return item; }
                }
            }
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate while discarding
        /// any value for which the filter returns null. Each element's index is
        /// used in the logic of the predicate function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence to filter.</param>
        /// <param name="predicate">A function to test each element for a condition;
        /// the second parameter of the function represents the index of the source element.</param>
        public static IEnumerable<TSource> WhereAny<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, int, bool?> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(predicate, nameof(predicate));

            return iterator();

            IEnumerable<TSource> iterator()
            {
                int index = 0;
                foreach (var item in source)
                {
                    var result = predicate(item, index);

                    if (result.HasValue && result.Value) { yield return item; }

                    index++;
                }
            }
        }
    }
}
