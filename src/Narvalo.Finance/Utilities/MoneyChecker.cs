// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Utilities
{
    using System;

    internal static class MoneyChecker
    {
        public static void ThrowIfCurrencyMismatch(Money mny, Currency cy)
        {
            if (mny.Currency != cy)
            {
                throw new InvalidOperationException("XXX");
            }
        }
    }
}
