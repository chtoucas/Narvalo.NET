// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System.Collections.Generic;
    using System.Linq;

    public static partial class Qperators
    {
        public static IEnumerable<TSource> CollectAny<TSource>(
            this IEnumerable<TSource> source)
            where TSource : class
        {
            Require.NotNull(source, nameof(source));

            return source.Where(x => x != null);
        }

        public static IEnumerable<TSource> CollectAny<TSource>(
            this IEnumerable<TSource?> source)
            where TSource : struct
        {
            Require.NotNull(source, nameof(source));

            return iterator();

            // Identical to: source.Where(x => x.HasValue).Select(x => x.Value);
            IEnumerable<TSource> iterator()
            {
                foreach (var item in source)
                {
                    if (item.HasValue) { yield return item.Value; }
                }
            }
        }
    }
}
