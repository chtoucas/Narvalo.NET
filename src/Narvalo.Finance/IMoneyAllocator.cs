// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System.Collections.Generic;

    // Distribute.
    //
    // The operation is not reversible, that is (in general):
    // > money.Distribute(count).Sum() != money;
    //
    // The decimal type is a floating number (even if .NET does not call it so, it still is).
    //
    // With or without rounding, the last element of the resulting collection is calculated
    // differently.
    // If you prefer:
    // > seq = money.Distribute(count).Reverse();
    public interface IMoneyAllocator
    {
        IEnumerable<Money> Distribute(Money money, int count);

        IEnumerable<Money> Distribute(Money money, RatioArray ratios);
    }
}
