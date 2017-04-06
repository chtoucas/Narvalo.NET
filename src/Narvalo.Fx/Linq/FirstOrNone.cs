// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;

    // For IEnumerable<T?>, prefer FirstOrDefault() over FirstOrNone().
    public static partial class Sequence
    {
        /// <summary>
        /// Returns the first element of a sequence, or <see cref="Maybe{TSource}.None"/>
        /// if the sequence contains no elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        public static Maybe<TSource> FirstOrNone<TSource>(this IEnumerable<TSource> source)
        {
            Require.NotNull(source, nameof(source));

            // Fast track.
            if (source is IList<TSource> list)
            {
                return list.Count > 0 ? Maybe.Of(list[0]) : Maybe<TSource>.None;
            }

            // Slow track.
            using (var iter = source.GetEnumerator())
            {
                return iter.MoveNext() ? Maybe.Of(iter.Current) : Maybe<TSource>.None;
            }
        }

        /// <summary>
        /// Returns the first element of a sequence that satisfies the
        /// <paramref name="predicate"/>, or <see cref="Maybe{TSource}.None"/>
        /// if no such element is found.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        public static Maybe<TSource> FirstOrNone<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(predicate, nameof(predicate));

            var seq = source.Where(predicate);

            using (var iter = seq.GetEnumerator())
            {
                return iter.MoveNext() ? Maybe.Of(iter.Current) : Maybe<TSource>.None;
            }
        }
    }
}
