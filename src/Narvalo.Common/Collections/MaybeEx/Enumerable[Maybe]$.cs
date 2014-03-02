// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Collections.MaybeEx
{
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Fx;

    public static partial class EnumerableExtensions
    {
        internal static Maybe<IEnumerable<TSource>> CollectCore<TSource>(this IEnumerable<Maybe<TSource>> @this)
        {
            IList<TSource> list = new List<TSource>();

            foreach (var m in @this) {
                if (!m.IsSome) {
                    return Maybe<IEnumerable<TSource>>.None;
                }

                list.Add(m.Value);
            }

            return Maybe.Create(list.AsEnumerable());
        }
    }
}
