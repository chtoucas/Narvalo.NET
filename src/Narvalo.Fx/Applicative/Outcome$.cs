// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Outcome{T}"/> and
    /// for querying objects that implement <see cref="IEnumerable{T}"/> where T is of type
    /// <see cref="Outcome{S}"/>.
    /// </summary>
    public static partial class OutcomeExtensions { }

    // Provides extension methods for IEnumerable<Outcome<T>>.
    public static partial class OutcomeExtensions
    {
        public static IEnumerable<TSource> CollectAny<TSource>(this IEnumerable<Outcome<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));

            return CollectAnyIterator(@this);
        }

        private static IEnumerable<TSource> CollectAnyIterator<TSource>(IEnumerable<Outcome<TSource>> source)
        {
            Demand.NotNull(source);

            foreach (var item in source)
            {
                if (item.IsSuccess) { yield return item.Value; }
            }
        }
    }
}
