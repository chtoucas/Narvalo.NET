// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Applicative;

    // For IEnumerable<T?>, prefer LastOrDefault() over LastOrNone().
    public static partial class Qperators
    {
        /// <summary>
        /// Returns the last element of a sequence, or <see cref="Maybe{TSource}.None"/>
        /// if the sequence contains no elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        public static Maybe<TSource> LastOrNone<TSource>(this IEnumerable<TSource> source)
        {
            Require.NotNull(source, nameof(source));

            // Fast track.
            if (source is IList<TSource> list)
            {
                return list.Count > 0 ? Maybe.Of(list[list.Count - 1]) : Maybe<TSource>.None;
            }

            // Slow track.
            using (var iter = source.GetEnumerator())
            {
                if (!iter.MoveNext()) { return Maybe<TSource>.None; }

                TSource item;
                do
                {
                    item = iter.Current;
                }
                while (iter.MoveNext());

                return Maybe.Of(item);
            }
        }

        /// <summary>
        /// Returns the last element of a sequence that satisfies the
        /// <paramref name="predicate"/>, or <see cref="Maybe{TSource}.None"/>
        /// if no such element is found.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        public static Maybe<TSource> LastOrNone<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(predicate, nameof(predicate));

            // Fast track.
            if (source is IList<TSource> list)
            {
                for (int i = list.Count - 1; i >= 0; --i)
                {
                    TSource item = list[i];
                    if (predicate(item))
                    {
                        return Maybe.Of(item);
                    }
                }

                return Maybe<TSource>.None;
            }

            // Slow track.
            using (var iter = source.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    TSource item = iter.Current;
                    if (predicate(item))
                    {
                        while (iter.MoveNext())
                        {
                            TSource element = iter.Current;
                            if (predicate(element))
                            {
                                item = element;
                            }
                        }

                        return Maybe.Of(item);
                    }
                }
            }

            return Maybe<TSource>.None;
        }
    }
}
