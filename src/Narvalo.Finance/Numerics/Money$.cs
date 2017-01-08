// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    public static class MoneyExtensions
    {
        public static Money Normalize(this Money @this, IDecimalRounding rounding)
        {
            Expect.NotNull(rounding);
            if (@this.IsNormalized) { return @this; }
            return MoneyFactory.Create(@this.Amount, @this.Currency, rounding);
        }
    }
}
