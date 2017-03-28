// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Applicative;

    public static partial class Qperators
    {
        public static Maybe<TSource> LastOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            Require.NotNull(@this, nameof(@this));

            // Fast track.
            if (@this is IList<TSource> list)
            {
                return list.Count > 0 ? Maybe.Of(list[list.Count - 1]) : Maybe<TSource>.None;
            }

            // Slow track.
            using (var iter = @this.GetEnumerator())
            {
                if (!iter.MoveNext()) { return Maybe<TSource>.None; }

                TSource item;
                do
                {
                    item = iter.Current;
                }
                while (iter.MoveNext());

                return Maybe.Of(item);
            }
        }

        public static Maybe<TSource> LastOrNone<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));

            // Fast track.
            if (@this is IList<TSource> list)
            {
                for (int i = list.Count - 1; i >= 0; --i)
                {
                    TSource item = list[i];
                    if (predicate(item))
                    {
                        return Maybe.Of(item);
                    }
                }

                return Maybe<TSource>.None;
            }

            // Slow track.
            using (var iter = @this.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    TSource item = iter.Current;
                    if (predicate(item))
                    {
                        while (iter.MoveNext())
                        {
                            TSource element = iter.Current;
                            if (predicate(element))
                            {
                                item = element;
                            }
                        }

                        return Maybe.Of(item);
                    }
                }
            }

            return Maybe<TSource>.None;
        }
    }
}
