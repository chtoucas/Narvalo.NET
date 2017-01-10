// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Rounding
{
    public static class MoneyExtensions
    {
        public static Money Normalize(this Money @this, IRoundingAdjuster adjuster)
        {
            Expect.NotNull(adjuster);
            if (@this.IsNormalized) { return @this; }
            return MoneyCreator.Create(@this.Amount, @this.Currency, adjuster);
        }
    }
}
