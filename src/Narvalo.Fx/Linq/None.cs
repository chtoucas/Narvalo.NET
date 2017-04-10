// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;

    public static partial class Qperators
    {
        // Returns true if no element in the sequence satisfies the predicate; otherwise false.
        public static bool None<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(predicate, nameof(predicate));

            // Same as !source.Any(predicate);
            // Same as source.All(x => !predicate(x));
            foreach (var element in source)
            {
                if (predicate(element))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
