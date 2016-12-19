// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Globalization;

    using Narvalo.Finance.Properties;

    internal sealed class MoneyFormatter
    {
        private const string DefaultFormat = "C";

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
                format = DefaultFormat;
            }

            // TODO: Improve C2. Add arbitrary formatting "###"? Check length?
            // http://www.guysmithferrier.com/post/2007/07/Displaying-Currencies-In-Your-Local-Format.aspx
            // https://codeascraft.com/2016/04/19/how-etsy-formats-currency/
            switch (format)
            {
                case "G":
                case "g":
                    // General: use the currency format "C" for the underlying NumberFormatInfo,
                    // but replace the currency symbol by the currency code.
                    return FormatGeneral(amount, currency, mfi);
                case "C":
                case "c":
                    // General: use the currency format "C" for the underlying NumberFormatInfo,
                    // but override the currency symbol if necessary.
                    return FormatCurrency(amount, currency, "0:C2", mfi);
                case "C2":
                case "c2":
                    // General: use the currency format "C2" for the underlying NumberFormatInfo,
                    // but override the currency symbol if necessary.
                    return FormatCurrency(amount, currency, "0:C", mfi);
                case "L":
                case "l":
                    // Legal.
                    // http://publications.europa.eu/code/en/en-370303.htm
                    return Narvalo.Format.Current("{1}\u00A0{0:F2}", amount, currency.Code);
                case "N":
                case "n":
                    // Neutral.
                    return Narvalo.Format.Current("{0:F2} ({1})", amount, currency.Code);
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

            var nfi = (NumberFormatInfo)mfi.NumberFormat.Clone();
            nfi.CurrencySymbol = currency.Code;

            return amount.ToString("C", mfi.NumberFormat);
        }

        private static string FormatCurrency(decimal amount, Currency currency, string format, MoneyFormatInfo mfi)
        {
            Require.NotNull(mfi, nameof(mfi));
            Demand.NotNull(currency);
            Warrant.NotNull<string>();

            if (mfi.CurrencyIsNative)
            {
                return amount.ToString(format, mfi.NumberFormat);
            }
            else
            {
                var nfi = (NumberFormatInfo)mfi.NumberFormat.Clone();
                nfi.CurrencySymbol = CurrencySymbol.GetFallbackSymbol(currency.Code);

                return amount.ToString(format, mfi.NumberFormat);
            }
        }
    }
}
