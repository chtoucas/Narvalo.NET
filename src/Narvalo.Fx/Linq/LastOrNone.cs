// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static partial class Qperators
    {
        public static Maybe<TSource> LastOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            Expect.NotNull(@this);

            return @this.Reverse().EmptyIfNull().FirstOrNone();
        }

        public static Maybe<TSource> LastOrNone<TSource>(this IEnumerable<TSource> @this, Func<TSource, bool> predicate)
        {
            Expect.NotNull(@this);
            Expect.NotNull(predicate);

            return @this.Reverse().EmptyIfNull().FirstOrNone(predicate);
        }
    }
}
