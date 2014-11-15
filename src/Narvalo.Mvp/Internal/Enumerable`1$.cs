// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides extension methods for <see cref="System.Collections.Generic.IEnumerable{T}"/>.
    /// </summary>
    internal static partial class EnumerableExtensions
    {
        public static bool IsEmpty<TSource>(this IEnumerable<TSource> @this)
        {
            Require.Object(@this);

            return !@this.Any();
        }

        public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> @this, TSource element)
        {
            Require.Object(@this);

            return @this.Concat(Enumerable.Repeat(element, 1));
        }
    }
}