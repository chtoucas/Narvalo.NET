// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Collections
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods for <c>IEnumerable&lt;Maybe&lt;T&gt;&gt;</c>.
    /// </summary>
    public static partial class EnumerableMaybeExtensions { }

    /// <content>
    /// Provides overrides for a bunch of auto-generated (extension) methods (see Maybe.g.cs).
    /// </content>
    public static partial class EnumerableMaybeExtensions
    {
        // Custom version of CollectCore.
        internal static Maybe<IEnumerable<TSource>> CollectCore<TSource>(this IEnumerable<Maybe<TSource>> @this)
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<Maybe<IEnumerable<TSource>>>() != null);

            var list = new List<TSource>();

            foreach (var m in @this) {
                // REVIEW: Is this the correct behaviour when m is null?
                if (m == null || m.IsNone) {
                    return Maybe<IEnumerable<TSource>>.None;
                }

                list.Add(m.Value);
            }

            return Maybe.Create(list.AsEnumerable());
        }
    }
}
