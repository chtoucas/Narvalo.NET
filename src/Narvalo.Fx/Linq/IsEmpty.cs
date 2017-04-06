// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System.Collections.Generic;
    using System.Linq;

    public static partial class Sequence
    {
        /// <summary>
        /// Returns true if the sequence is empty; otherwise false.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        public static bool IsEmpty<TSource>(this IEnumerable<TSource> source)
            => !source.Any();
    }
}
