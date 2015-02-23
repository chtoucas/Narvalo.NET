// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Collections
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Narvalo.Fx;

    // Optimized extensions.
    public static partial class EnumerableOutputExtensions
    {
        internal static Output<IEnumerable<TSource>> CollectCore<TSource>(this IEnumerable<Output<TSource>> @this)
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<Output<IEnumerable<TSource>>>() != null);

            var list = new List<TSource>();

            foreach (var m in @this) {
                // REVIEW: Is this the correct behaviour when m is null?
                if (m == null) {
                    continue;
                }

                if (m.IsFailure) {
                    return Output.Failure<IEnumerable<TSource>>(m.ExceptionInfo);
                }

                list.Add(m.Value);
            }

            return Output.Success(list.AsEnumerable());
        }
    }
}
