// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Collections
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Narvalo.Fx;

    // Optimized extensions.
    public static partial class EnumerableMaybeExtensions
    {
        internal static Maybe<IEnumerable<TSource>> CollectCore<TSource>(this IEnumerable<Maybe<TSource>> @this)
        {
            DebugCheck.NotNull(@this);

            var list = new List<TSource>();

            foreach (var m in @this) {
                if (m.IsNone) {
                    return Maybe<IEnumerable<TSource>>.None;
                }

                list.Add(m.Value);
            }

            return Maybe.Create(list.AsEnumerable());
        }
    }
}
