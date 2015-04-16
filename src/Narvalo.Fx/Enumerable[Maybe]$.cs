// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// Provides overrides for a bunch of auto-generated (extension) methods (see Maybe.g.cs).
    /// </summary>
    public static partial class EnumerableMaybeExtensions
    {
        // Custom version of CollectCore.
        internal static Maybe<IEnumerable<TSource>> CollectCore<TSource>(this IEnumerable<Maybe<TSource>> @this)
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<Maybe<IEnumerable<TSource>>>() != null);

            var list = new List<TSource>();

            foreach (var m in @this)
            {
                // REVIEW: Is this the expected behaviour when m is null?
                if (m == null || !m.IsSome)
                {
                    return Maybe<IEnumerable<TSource>>.None;
                }

                list.Add(m.Value);
            }

            return Maybe.Of(list.AsEnumerable());
        }
    }
}
