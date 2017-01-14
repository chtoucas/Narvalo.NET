// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using Narvalo.Finance.Generic;
    using Narvalo.Finance.Properties;

    // Default formatter for all money-like types.
    public static class MoneyFormatter
    {
        private const string DEFAULT_FORMAT = "G";

        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames", MessageId = "1#", Justification = "[Intentionally] Standard .NET name with Format().")]
        public static string Format<TCurrency>(Money<TCurrency> money, string format, NumberFormatInfo numberFormat)
            where TCurrency : CurrencyUnit<TCurrency>
        {
            Warrant.NotNull<string>();

            // TODO: To be updated once Money<> handle normalization. See below.
            var nfi = (NumberFormatInfo)(numberFormat ?? NumberFormatInfo.CurrentInfo).Clone();
            nfi.NumberDecimalDigits = money.Unit.DecimalPlaces;
            string amount = money.Amount.ToString("N", nfi);

            return Format(amount, money.Unit.Code, format);
        }

        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames", MessageId = "1#", Justification = "[Intentionally] Standard .NET name with Format().")]
        public static string Format(Money money, string format, NumberFormatInfo numberFormat)
        {
            Warrant.NotNull<string>();

            string amount;

            if (money.IsNormalized && money.IsNormalizable)
            {
                // If the instance is normalizable and normalized, we override the number
                // of decimal places.
                var nfi = (NumberFormatInfo)(numberFormat ?? NumberFormatInfo.CurrentInfo).Clone();
                nfi.NumberDecimalDigits = money.Currency.DecimalPlaces;
                amount = money.Amount.ToString("N", nfi);
            }
            else
            {
                amount = money.Amount.ToString("N", numberFormat);
            }

            return Format(amount, money.Currency.Code, format);
        }

        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames", MessageId = "1#", Justification = "[Intentionally] Standard .NET name with Format().")]
        public static string Format(Moneypenny penny, string format, NumberFormatInfo numberFormat)
        {
            Warrant.NotNull<string>();

            string amount = penny.Amount.ToString("N", numberFormat ?? NumberFormatInfo.CurrentInfo);

            return Format(amount, penny.PennyOrCurrencyCode, format);
        }

        private static string Format(string amount, string currencyCode, string format)
        {
            Warrant.NotNull<string>();

            if (format == null || format.Length == 0)
            {
                format = DEFAULT_FORMAT;
            }

            switch (format)
            {
                case "N":
                case "n":
                    // Numeric. Does not include any information about the currency.
                    return amount;
                case "L":
                case "l":
                    // Left (Currency code placed on the).
                    return currencyCode + "\u00A0" + amount;
                case "R":
                case "r":
                case "G":
                case "g":
                    // General (default) or Right (Currency code placed on the).
                    return amount + "\u00A0" + currencyCode;
                default:
                    throw new FormatException(
                        Narvalo.Format.Current(Strings.Money_InvalidFormatSpecification));
            }
        }
    }
}
