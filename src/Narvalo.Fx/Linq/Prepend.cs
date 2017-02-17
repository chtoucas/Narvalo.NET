// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Linq
{
    using System.Collections.Generic;

    public static partial class Qperators
    {
        // See Append for remarks.
        public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> @this, TSource element)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<IEnumerable<TSource>>();

            return PrependIterator(@this, element);
        }

        private static IEnumerable<TSource> PrependIterator<TSource>(IEnumerable<TSource> source, TSource element)
        {
            Demand.NotNull(source);
            Warrant.NotNull<IEnumerable<TSource>>();

            yield return element;

            foreach (var item in source)
            {
                yield return item;
            }
        }
    }
}
