// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System.Collections.Generic;

    // The operation is not reversible, that is (in general):
    // > Allocate(money, count).Sum() != money;
    //
    // The decimal type is a floating number (even if .NET does not call it so, it still is).
    //
    // With or without rounding, the last element of the resulting collection is calculated
    // differently.
    // If you prefer:
    // > seq = Distribute(money, count).Reverse();
    public interface IMoneyAllocator
    {
        IEnumerable<Money> Allocate(Money money, int count);

        IEnumerable<Money> Allocate(Money money, RatioArray ratios);
    }
}
