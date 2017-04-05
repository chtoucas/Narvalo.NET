// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System.Collections.Generic;

    public static partial class Qperators
    {
        // There is a much better implementation coming soon (?).
        // https://github.com/dotnet/corefx/blob/master/src/System.Linq/src/System/Linq/AppendPrepend.cs
        // This is of particular important when calling Append or Prepend mutiple times in a row.
        public static IEnumerable<TSource> Append<TSource>(
            this IEnumerable<TSource> source,
            TSource element)
        {
            Require.NotNull(source, nameof(source));

            return Iterator();

            IEnumerable<TSource> Iterator()
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

            return Iterator();

            IEnumerable<TSource> Iterator()
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
