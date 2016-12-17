// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
    using System;

    // See https://msdn.microsoft.com/fr-fr/library/dwhawy9k(v=vs.110).aspx
    // and https://msdn.microsoft.com/en-us/library/txafckwd(v=vs.110).aspx
    internal static class MoneyFormatter
    {
        public static string Format(decimal amount, Currency currency)
        {
            Demand.NotNull(currency);

            return Narvalo.Format.Current("{0} {1:F2}", currency.Code, amount);
        }

        public static string Format(decimal amount, string format, IFormatProvider formatProvider)
        {
            return amount.ToString(format, formatProvider);
        }
    }
}
