// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Linq
{
    using System.Collections.Generic;
    using System.Linq;

    public static partial class Operators
    {
        // Useful when using built-in LINQ operators. Even if it is not publicly visible,
        // I believe that all LINQ operators never return a null but rather an empty sequence if needed.
        public static IEnumerable<TSource> EmptyIfNull<TSource>(this IEnumerable<TSource> @this)
        {
            Warrant.NotNull<IEnumerable<TSource>>();

            if (@this == null)
            {
                return Enumerable.Empty<TSource>();
            }

            return @this;
        }
    }
}
