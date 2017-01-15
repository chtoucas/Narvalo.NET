// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Allocators
{
    using System;
    using System.Collections.Generic;

    public static class MoneyExtensions
    {
        private static readonly IMoneyAllocator s_DefaultAllocator
            = new DefaultMoneyAllocator();

        public static IEnumerable<Money> Allocate(this Money @this, int count)
            => s_DefaultAllocator.Allocate(@this, count);

        public static IEnumerable<Money> Allocate(this Money @this, RatioArray ratios)
            => s_DefaultAllocator.Allocate(@this, ratios);

        public static IEnumerable<Money> Allocate(this Money @this, int count, MidpointRounding mode)
            => new MidpointRoundingMoneyAllocator(mode).Allocate(@this, count);

        public static IEnumerable<Money> Allocate(this Money @this, RatioArray ratios, MidpointRounding mode)
            => new MidpointRoundingMoneyAllocator(mode).Allocate(@this, ratios);

        public static IEnumerable<Money> Allocate(this Money @this, int count, IMoneyAllocator allocator)
        {
            Require.NotNull(allocator, nameof(allocator));
            return allocator.Allocate(@this, count);
        }

        public static IEnumerable<Money> Allocate(this Money @this, RatioArray ratios, IMoneyAllocator allocator)
        {
            Require.NotNull(allocator, nameof(allocator));
            return allocator.Allocate(@this, ratios);
        }
    }
}
