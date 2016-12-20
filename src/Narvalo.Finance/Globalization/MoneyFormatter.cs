// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Globalization;

    using Narvalo.Finance.Properties;

    internal sealed class MoneyFormatter
    {
        private const string DEFAULT_FORMAT = "G";

        public static string Format(Money money, string format, MoneyFormatInfo mfi)
        {
            Demand.NotNull(mfi);
            Warrant.NotNull<string>();

            return Format(money.Amount, money.Currency, format, mfi);
        }

        public static string Format(decimal amount, Currency currency, string format, MoneyFormatInfo mfi)
        {
            Demand.NotNull(currency);
            Demand.NotNull(mfi);
            Warrant.NotNull<string>();

            if (format == null || format.Length == 0)
            {
                format = DEFAULT_FORMAT;
            }

            switch (format)
            {
                case "G":
                case "g":
                    return FormatGeneral(amount, currency, mfi);
                case "C":
                case "c":
                    return FormatCurrency(amount, currency, mfi);
                default:
                    throw new FormatException(
                        Narvalo.Format.Current(Strings.Money_InvalidFormatSpecification));
            }
        }

        private static string FormatGeneral(decimal amount, Currency currency, MoneyFormatInfo mfi)
        {
            Require.NotNull(mfi, nameof(mfi));
            Demand.NotNull(currency);
            Warrant.NotNull<string>();

            // http://publications.europa.eu/code/en/en-370303.htm
            //return Narvalo.Format.Current("{1}\u00A0{0:F2}", amount, currency.Code);
            // CurrencyGroupSeparator
            // CurrencyDecimalSeparator
            var nfi = (NumberFormatInfo)mfi.NumberFormat.Clone();
            nfi.CurrencySymbol = currency.Code;

            return amount.ToString("C", nfi);
        }

        private static string FormatCurrency(decimal amount, Currency currency, MoneyFormatInfo mfi)
        {
            Require.NotNull(mfi, nameof(mfi));
            Demand.NotNull(currency);
            Warrant.NotNull<string>();

            if (mfi.CurrencyIsNative)
            {
                return amount.ToString("C", mfi.NumberFormat);
            }
            else
            {
                var nfi = (NumberFormatInfo)mfi.NumberFormat.Clone();
                nfi.CurrencySymbol = currency.Code;

                return amount.ToString("C", nfi);
            }
        }
    }
}
