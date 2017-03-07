// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Fallible{T}"/> and for querying
    /// objects that implement <see cref="IEnumerable{T}"/> where T is of type <see cref="Fallible{S}"/>.
    /// </summary>
    public static partial class FallibleExtensions { }

    // Provides extension methods for IEnumerable<Fallible<T>>.
    public static partial class FallibleExtensions
    {
        public static IEnumerable<TSource> CollectAny<TSource>(this IEnumerable<Fallible<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));

            return CollectAnyIterator(@this);
        }

        private static IEnumerable<TSource> CollectAnyIterator<TSource>(IEnumerable<Fallible<TSource>> source)
        {
            Demand.NotNull(source);

            foreach (var item in source)
            {
                if (item.IsSuccess) { yield return item.Value; }
            }
        }
    }
}
