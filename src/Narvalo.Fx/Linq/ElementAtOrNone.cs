// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System.Collections.Generic;

    using Narvalo.Applicative;

    // For IEnumerable<T?>, prefer ElementAtOrDefault() over ElementAtOrNone().
    public static partial class Sequence
    {
        /// <summary>
        /// Returns the element at the specified index in a sequence or
        /// <see cref="Maybe{TSource}.None"/> if the index is out of range.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        // Adapted from https://github.com/jskeet/edulinq/blob/master/src/Edulinq/ElementAt.cs
        public static Maybe<TSource> ElementAtOrNone<TSource>(
            this IEnumerable<TSource> source,
            int index)
        {
            Require.NotNull(source, nameof(source));

            if (index < 0) { return Maybe<TSource>.None; }

            // Fast track.
            if (source is ICollection<TSource> collection)
            {
                int count = collection.Count;
                if (index >= count) { return Maybe<TSource>.None; }

                if (source is IList<TSource> list)
                {
                    return Maybe.Of(list[index]);
                }
            }

            // Slow track.
            using (var iter = source.GetEnumerator())
            {
                // Note use of -1 so that we start off my moving onto element 0.
                // Don't want to use i <= index in case index == int.MaxValue!
                // We don't need to fetch the current value each time - get to the right place first.
                for (int i = -1; i < index; i++)
                {
                    if (!iter.MoveNext())
                    {
                        return Maybe<TSource>.None;
                    }
                }

                return Maybe.Of(iter.Current);
            }
        }
    }
}
