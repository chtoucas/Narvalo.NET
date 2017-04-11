// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;

    public static partial class Qperators
    {
        public static IEnumerable<TSource> WhereAny<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool?> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(predicate, nameof(predicate));

            return iterator();

            IEnumerable<TSource> iterator()
            {
                foreach (var item in source)
                {
                    var result = predicate(item);

                    if (result.HasValue && result.Value) { yield return item; }
                }
            }
        }
    }
}
