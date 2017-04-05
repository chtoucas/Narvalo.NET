// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Outcome{T}"/> and
    /// for querying objects that implement <see cref="IEnumerable{T}"/> where T is of type
    /// <see cref="Outcome{S}"/>.
    /// </summary>
    public static partial class OutcomeExtensions { }

    // Provides extension methods for IEnumerable<Outcome<T>>.
    public static partial class OutcomeExtensions
    {
        public static IEnumerable<TSource> CollectAny<TSource>(
            this IEnumerable<Outcome<TSource>> source)
        {
            Require.NotNull(source, nameof(source));

            return CollectAnyIterator(source);
        }

        private static IEnumerable<TSource> CollectAnyIterator<TSource>(
            IEnumerable<Outcome<TSource>> source)
        {
            Debug.Assert(source != null);

            foreach (var item in source)
            {
                if (item.IsSuccess) { yield return item.Value; }
            }
        }
    }
}
