// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;

    // For IEnumerable<T?>, prefer SingleOrDefault() over SingleOrNone().
    public static partial class Qperators
    {
        /// <summary>
        /// Returns the only element of a sequence, or <see cref="Maybe{TSource}.None"/>
        /// if the sequence is empty or contains more than one element.
        /// <para>Here we differ in behaviour from the standard query SingleOrDefault which
        /// throws an exception if there is more than one element in the sequence.</para>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        public static Maybe<TSource> SingleOrNone<TSource>(this IEnumerable<TSource> source)
        {
            Require.NotNull(source, nameof(source));

            // Fast track.
            if (source is IList<TSource> list)
            {
                return list.Count == 1 ? Maybe.Of(list[0]) : Maybe<TSource>.None;
            }

            // Slow track.
            using (var iter = source.GetEnumerator())
            {
                // Return None if the sequence is empty.
                if (!iter.MoveNext()) { return Maybe<TSource>.None; }

                var item = iter.Current;

                // Return None if there is one more element.
                return iter.MoveNext() ? Maybe<TSource>.None : Maybe.Of(item);
            }
        }

        /// <summary>
        /// Returns the only element of a sequence that satisfies
        /// a specified predicate, or <see cref="Maybe{TSource}.None"/>
        /// if no such element exists or there are more than one of them.
        /// <para>Here we differ in behaviour from the standard query SingleOrDefaultwhich
        /// throws an exception if more than one element satisfies the predicate.</para>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        public static Maybe<TSource> SingleOrNone<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(predicate, nameof(predicate));

            var seq = source.Where(predicate);

            using (var iter = seq.GetEnumerator())
            {
                // Return None if the sequence is empty.
                if (!iter.MoveNext()) { return Maybe<TSource>.None; }

                var item = iter.Current;

                // Return None if there is one more element.
                return iter.MoveNext() ? Maybe<TSource>.None : Maybe.Of(item);
            }
        }
    }
}
