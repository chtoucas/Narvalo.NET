// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static partial class Qperators
    {
        public static TAccumulate AggregateBack<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            Expect.NotNull(@this);
            Expect.NotNull(accumulator);

            return @this.Reverse().Aggregate(seed, accumulator);
        }

        public static TSource AggregateBack<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, TSource> accumulator)
        {
            Expect.NotNull(@this);
            Expect.NotNull(accumulator);

            return @this.Reverse().Aggregate(accumulator);
        }
    }
}
