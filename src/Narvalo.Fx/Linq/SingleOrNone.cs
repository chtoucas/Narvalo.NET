// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static partial class Qperators
    {
        public static Maybe<TSource> SingleOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            Require.NotNull(@this, nameof(@this));

            using (var iter = @this.EmptyIfNull().GetEnumerator())
            {
                // Return None if the sequence is empty.
                var result = iter.MoveNext() ? Maybe.Of(iter.Current) : Maybe<TSource>.None;

                // Return None if there is one more element.
                return iter.MoveNext() ? Maybe<TSource>.None : result;
            }
        }

        public static Maybe<TSource> SingleOrNone<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));

            var seq = from t in @this where predicate.Invoke(t) select t;

            using (var iter = seq.EmptyIfNull().GetEnumerator())
            {
                // Return None if the sequence is empty.
                var result = iter.MoveNext() ? Maybe.Of(iter.Current) : Maybe<TSource>.None;

                // Return None if there is one more element.
                return iter.MoveNext() ? Maybe<TSource>.None : result;
            }
        }
    }
}
