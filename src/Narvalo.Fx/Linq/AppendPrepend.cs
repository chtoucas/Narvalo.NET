// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Linq
{
    using System.Collections.Generic;

    public static partial class Qperators
    {
        // There is a much better implementation coming soon (?).
        // https://github.com/dotnet/corefx/commits/master/src/System.Linq/src/System/Linq/AppendPrepend.cs
        // This is especially important when calling Append or Prepend mutiple times in a row,
        // which is indeed the case with Collect() and WhereBy().
        public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> @this, TSource element)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<IEnumerable<TSource>>();

            return AppendIterator(@this, element);
        }

        public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> @this, TSource element)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<IEnumerable<TSource>>();

            return PrependIterator(@this, element);
        }

        private static IEnumerable<TSource> AppendIterator<TSource>(IEnumerable<TSource> source, TSource element)
        {
            Demand.NotNull(source);
            Warrant.NotNull<IEnumerable<TSource>>();

            foreach (var item in source)
            {
                yield return item;
            }

            yield return element;
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
