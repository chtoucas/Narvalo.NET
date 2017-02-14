// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Linq
{
    using System.Collections.Generic;

    public static partial class Operators
    {
        // There is a much better implementation coming soon (?).
        // https://github.com/dotnet/corefx/commits/master/src/System.Linq/src/System/Linq/AppendPrepend.cs
        // REVIEW: Since this method is a critical component of Collect(), we could include the
        // .NET implementation until it is publicly available.
        public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> @this, TSource element)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<IEnumerable<TSource>>();

            return AppendIterator(@this, element);
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

        // See remarks for Append.
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
