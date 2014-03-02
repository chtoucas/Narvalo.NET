// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Collections
{
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Fx;

    // Optimized extensions.
    public static partial class EnumerableExtensions
    {
        internal static Output<IEnumerable<TSource>> CollectCore<TSource>(this IEnumerable<Output<TSource>> @this)
        {
            IList<TSource> list = new List<TSource>();

            foreach (var m in @this) {
                if (m.IsFailure) {
                    return Output.Failure<IEnumerable<TSource>>(m.ExceptionInfo);
                }

                list.Add(m.Value);
            }

            return Output.Success(list.AsEnumerable());
        }
    }
}
