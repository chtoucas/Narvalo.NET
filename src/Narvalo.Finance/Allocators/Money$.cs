// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Allocators
{
    using System;
    using System.Collections.Generic;

    public static class MoneyExtensions
    {
        public static IEnumerable<Money> Allocate(this Money @this, int count)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<Money> Allocate(this Money @this, RatioArray ratios)
        {
            throw new NotImplementedException();
        }

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
