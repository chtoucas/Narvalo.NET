// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System.Collections.Generic;

    public static partial class Qperators
    {
        public static IEnumerable<TSource> Flatten<TSource>(
            this IEnumerable<IEnumerable<TSource>> source)
        {
            Require.NotNull(source, nameof(source));

            return iterator();

            IEnumerable<TSource> iterator()
            {
                foreach (IEnumerable<TSource> inner in source)
                {
                    if (inner == null) { continue; }

                    foreach (TSource item in inner)
                    {
                        yield return item;
                    }
                }
            }

        }
    }
}
