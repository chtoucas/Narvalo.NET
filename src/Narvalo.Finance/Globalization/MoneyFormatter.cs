// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Globalization;

    using Narvalo.Finance.Properties;

    // Currency formatting is difficult. This class works consistently across .NET cultures
    // at the sacrifice of omitting the currency symbol; it still uses the culture's rules to
    // format the amount.
    public static class MoneyFormatter
    {
        private const string DEFAULT_FORMAT = "G";

        public static string Format<TCurrency>(Money<TCurrency> money, string format, NumberFormatInfo nfi)
            where TCurrency : Currency
        {
            Warrant.NotNull<string>();
            return Format(money.Amount, Money<TCurrency>.UnderlyingCurrency, format, nfi);
        }

        public static string Format(Money money, string format, NumberFormatInfo nfi)
        {
            Warrant.NotNull<string>();
            return Format(money.Amount, money.Currency, format, nfi);
        }

        private static string Format(decimal amount, Currency currency, string format, NumberFormatInfo nfi)
        {
            Demand.NotNull(nfi);
            Warrant.NotNull<string>();

            if (format == null || format.Length == 0)
            {
                format = DEFAULT_FORMAT;
            }

            switch (format)
            {
                case "N":
                case "n":
                    // Numeric. Do not display any currency symbol at all.
                    return amount.ToString("C", nfi.GetNoSymbolNoSpaceClone());
                case "G":
                case "g":
                    // General (default). It replaces the currency symbol by the currency code
                    // and ensures that there is a space between the amount and the currency code.
                    return amount.ToString("C", nfi.GetCurrencyCodeAndSpaceClone(currency));
                default:
                    throw new FormatException(
                        Narvalo.Format.Current(Strings.Money_InvalidFormatSpecification));
            }
        }
    }
}
