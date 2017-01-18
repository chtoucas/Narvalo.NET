// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Globalization;

    using Narvalo.Finance.Generic;
    using Narvalo.Finance.Internal;
    using Narvalo.Finance.Properties;

    // Default formatter for all money-like types.
    public static class MoneyFormatter
    {
        private const string NO_BREAK_SPACE = "\u00A0";

        public static string FormatMoney<TCurrency>(
            Money<TCurrency> money,
            string format,
            NumberFormatInfo numberFormat)
            where TCurrency : Currency<TCurrency>
        {
            Warrant.NotNull<string>();

            var fmt = FormatParts.Parse(format, money.DecimalPrecision);
            string amount = money.Amount.ToString(fmt.AmountFormat, numberFormat ?? NumberFormatInfo.CurrentInfo);
            return FormatImpl(amount, money.Currency.Code, fmt);
        }

        public static string FormatMoney(Money money, string format, NumberFormatInfo numberFormat)
        {
            Warrant.NotNull<string>();

            var fmt = FormatParts.Parse(format, money.DecimalPrecision);
            string amount = money.Amount.ToString(fmt.AmountFormat, numberFormat ?? NumberFormatInfo.CurrentInfo);
            return FormatImpl(amount, money.Currency.Code, fmt);
        }

        public static string FormatPenny(Moneypenny penny, string format, NumberFormatInfo numberFormat)
        {
            Warrant.NotNull<string>();

            var fmt = FormatParts.Parse(format, 0);
            string amount = penny.Amount.ToString(fmt.AmountFormat, numberFormat ?? NumberFormatInfo.CurrentInfo);
            return FormatImpl(amount, penny.PennyOrCurrencyCode, fmt);
        }

        private static string FormatImpl(string amount, string currencyCode, FormatParts format)
        {
            Warrant.NotNull<string>();

            switch (format.MainFormat)
            {
                case 'N':
                    // Numeric. Does not include any information about the currency.
                    return amount;
                case 'L':
                    // Left (Currency code placed on the).
                    return currencyCode + NO_BREAK_SPACE + amount;
                case 'R':
                case 'G':
                    // General (default) or Right (Currency code placed on the).
                    return amount + NO_BREAK_SPACE + currencyCode;
                default:
                    throw new FormatException(Format.Current(Strings.Money_InvalidFormatSpecification));
            }
        }
    }
}
