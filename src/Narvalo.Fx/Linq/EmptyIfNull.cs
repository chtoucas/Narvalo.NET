// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System.Collections.Generic;
    using System.Linq;

    public static partial class Sequence
    {
        /// <summary>
        /// Returns a new empty sequence if the sequence is empty; otherwise
        /// it returns the sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        public static IEnumerable<TSource> EmptyIfNull<TSource>(this IEnumerable<TSource> source)
            => source ?? Enumerable.Empty<TSource>();
    }
}
