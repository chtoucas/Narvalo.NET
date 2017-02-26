// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static partial class Qperators
    {
        // Named <c>listToMaybe</c> in Haskell parlance.
        public static Maybe<TSource> FirstOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            Require.NotNull(@this, nameof(@this));

            // Fast track.
            var list = @this as IList<TSource>;
            if (list != null)
            {
                return list.Count > 0 ? Maybe.Of(list[0]) : Maybe<TSource>.None;
            }

            // Slow track.
            using (var iter = @this.GetEnumerator())
            {
                return iter.MoveNext() ? Maybe.Of(iter.Current) : Maybe<TSource>.None;
            }
        }

        public static Maybe<TSource> FirstOrNone<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));

            var seq = @this.Where(predicate);

            using (var iter = seq.GetEnumerator())
            {
                return iter.MoveNext() ? Maybe.Of(iter.Current) : Maybe<TSource>.None;
            }
        }
    }
}
