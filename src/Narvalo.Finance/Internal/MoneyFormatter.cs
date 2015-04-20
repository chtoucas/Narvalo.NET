// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
    using System;
    using System.Diagnostics.Contracts;

    internal static class MoneyFormatter
    {
        public static string Format(decimal amount, Currency currency)
        {
            Contract.Requires(currency != null);

            return Narvalo.Format.CurrentCulture("{0} {1:F2}", currency.Code, amount);
        }

        public static string Format(decimal amount, string format, IFormatProvider formatProvider)
        {
            return amount.ToString(format, formatProvider);
        }
    }
}
