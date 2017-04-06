// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System.Collections.Generic;

    public static partial class Qperators
    {
        public static IEnumerable<TSource> Append<TSource>(
            this IEnumerable<TSource> source,
            TSource element)
        {
            Require.NotNull(source, nameof(source));

            return iterator();

            IEnumerable<TSource> iterator()
            {
                foreach (var item in source)
                {
                    yield return item;
                }

                yield return element;
            }
        }

        public static IEnumerable<TSource> Prepend<TSource>(
            this IEnumerable<TSource> source,
            TSource element)
        {
            Require.NotNull(source, nameof(source));

            return iterator();

            IEnumerable<TSource> iterator()
            {
                yield return element;

                foreach (var item in source)
                {
                    yield return item;
                }
            }
        }
    }
}
