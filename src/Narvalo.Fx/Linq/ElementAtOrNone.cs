// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Linq
{
    using System.Collections.Generic;

    public static partial class Qperators
    {
        // Adapted from https://github.com/jskeet/edulinq/blob/master/src/Edulinq/ElementAt.cs
        public static Maybe<TSource> ElementAtOrNone<TSource>(this IEnumerable<TSource> @this, int index)
        {
            Require.NotNull(@this, nameof(@this));

            if (index < 0) { return Maybe<TSource>.None; }

            // Fast track.
            var collection = @this as ICollection<TSource>;
            if (collection != null)
            {
                int count = collection.Count;
                if (index >= count) { return Maybe<TSource>.None; }

                var list = @this as IList<TSource>;
                if (list != null)
                {
                    return Maybe.Of(list[index]);
                }
            }

            // Slow track.
            using (var iter = @this.GetEnumerator())
            {
                // Note use of -1 so that we start off my moving onto element 0.
                // Don't want to use i <= index in case index == int.MaxValue!
                // We don't need to fetch the current value each time - get to the right place first.
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
