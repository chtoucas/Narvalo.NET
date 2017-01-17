// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Globalization;

    using Narvalo.Finance.Generic;
    using Narvalo.Finance.Properties;

    // Default formatter for all money-like types.
    public static class MoneyFormatter
    {
        private const string AMOUNT_FORMAT = "N";
        private const string DEFAULT_FORMAT = "G";
        private const string NO_BREAK_SPACE = "\u00A0";

        public static string FormatMoney<TCurrency>(
            Money<TCurrency> money,
            string format,
            NumberFormatInfo numberFormat)
            where TCurrency : Currency<TCurrency>
        {
            Warrant.NotNull<string>();

            // TODO: To be updated once Money<> handles normalization too. See below.
            string amount = money.Amount.ToString(AMOUNT_FORMAT, numberFormat ?? NumberFormatInfo.CurrentInfo);

            return FormatImpl(amount, money.Unit.Code, format);
        }

        public static string FormatMoney(Money money, string format, NumberFormatInfo numberFormat)
        {
            Warrant.NotNull<string>();

            NumberFormatInfo nfi;
            if (money.IsNormalized && money.IsRoundable)
            {
                nfi = (numberFormat ?? NumberFormatInfo.CurrentInfo).Copy();
                nfi.NumberDecimalDigits = money.Currency.DecimalPlaces;
            }
            else
            {
                nfi = numberFormat ?? NumberFormatInfo.CurrentInfo;
            }

            string amount = money.Amount.ToString(AMOUNT_FORMAT, nfi);

            return FormatImpl(amount, money.Currency.Code, format);
        }

        public static string FormatPenny(Moneypenny penny, string format, NumberFormatInfo numberFormat)
        {
            Warrant.NotNull<string>();

            // Override the default number of decimal digits which is 2.
            var nfi = (numberFormat ?? NumberFormatInfo.CurrentInfo).Copy();
            nfi.NumberDecimalDigits = 0;

            string amount = penny.Amount.ToString(AMOUNT_FORMAT, nfi);

            return FormatImpl(amount, penny.PennyOrCurrencyCode, format);
        }

        private static string FormatImpl(string amount, string currencyCode, string format)
        {
            Warrant.NotNull<string>();

            if (format == null || format.Length == 0) { format = DEFAULT_FORMAT; }
            if (format.Length != 1) { throw new FormatException("XXX"); }

            // Take the first char and uppercase it (ASCII only).
            switch (format[0] & 0xDF)
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
