// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Linq
{
    using System.Collections;
    using System.Collections.Generic;

    public static partial class Qperators
    {
        // Adapted from https://github.com/jskeet/edulinq/blob/master/src/Edulinq/ElementAt.cs
        public static Maybe<TSource> ElementAtOrNone<TSource>(this IEnumerable<TSource> @this, int index)
        {
            Require.NotNull(@this, nameof(@this));

            if (index < 0) { return Maybe<TSource>.None; }

            var collection = @this as ICollection<TSource>;
            if (collection != null)
            {
                int count = collection.Count;
                if (index >= count) { return Maybe<TSource>.None; }

                // If it's a list, we know we're okay now - just return directly...
                var list = @this as IList<TSource>;
                if (list != null)
                {
                    return Maybe.Of(list[index]);
                }
                // Okay, non-list collection: we'll have to iterate, but at least
                // we've caught any invalid index values early.
            }

            // For non-generic collections all we can do is an early bounds check.
            var nonGenericCollection = @this as ICollection;
            if (nonGenericCollection != null)
            {
                int count = nonGenericCollection.Count;
                if (index >= count) { return Maybe<TSource>.None; }
            }

            // We don't need to fetch the current value each time - get to the right place first.
            using (var iter = @this.GetEnumerator())
            {
                // Note use of -1 so that we start off my moving onto element 0.
                // Don't want to use i <= index in case index == int.MaxValue!
                for (int i = -1; i < index; i++)
                {
                    if (!iter.MoveNext())
                    {
                        return Maybe<TSource>.None;
                    }
                }

                return Maybe.Of(iter.Current);
            }
        }
    }
}
