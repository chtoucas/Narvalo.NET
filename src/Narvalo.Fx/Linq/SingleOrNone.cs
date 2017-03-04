// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;

    public static partial class Qperators
    {
        // WARNING: Here we defer in behaviour from LINQ.
        // If there is more than one element, we do not throw but rather return None.
        public static Maybe<TSource> SingleOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            Require.NotNull(@this, nameof(@this));

            // Fast track.
            var list = @this as IList<TSource>;
            if (list != null)
            {
                return list.Count == 1 ? Maybe.Of(list[0]) : Maybe<TSource>.None;
            }

            // Slow track.
            using (var iter = @this.GetEnumerator())
            {
                // Return None if the sequence is empty.
                if (!iter.MoveNext()) { return Maybe<TSource>.None; }

                var item = iter.Current;

                // Return None if there is one more element.
                return iter.MoveNext() ? Maybe<TSource>.None : Maybe.Of(item);
            }
        }

        // WARNING: Here we defer in behaviour from LINQ.
        // If there is more than one element, we do not throw but rather return None.
        public static Maybe<TSource> SingleOrNone<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));

            var seq = @this.Where(predicate);

            using (var iter = seq.GetEnumerator())
            {
                // Return None if the sequence is empty.
                if (!iter.MoveNext()) { return Maybe<TSource>.None; }

                var item = iter.Current;

                // Return None if there is one more element.
                return iter.MoveNext() ? Maybe<TSource>.None : Maybe.Of(item);
            }
        }
    }
}
