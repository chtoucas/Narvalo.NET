// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Globalization;

    using Narvalo.Finance.Generic;
    using Narvalo.Finance.Properties;

    // Default formatter for all money-like types:
    // - The amount is formatted using the Number specifier ("N") for the requested culture.
    // - The position of the currency code depends on the format specifier
    //   (on the left w/ "L"; on the right w/ "R" or "G"; not-included w/ "N").
    //
    // A standard money format string takes the form "Axx", where:
    // - A is a single alphabetic character called the format specifier.
    //   Admissible values are "N" (Numeric), "L" (Left), "R" (Right) and "G" (General):
    //   * "N", do not include any information about the currency.
    //   * "L", place the currency code on the left of the amount.
    //   * "R" and "G", place the currency code on the right of the amount.
    // - xx is an optional integer called the precision specifier. The precision specifier ranges
    //   from 0 to 99 and affects the number of digits displayed for the amount.
    //   If no precision is given, we use the decimal precision reported by the object
    //   (DecimalPrecision for Money and Money<T> and 0 for Moneypenny).
    //   For Money and Money<T>, if DecimalPrecision is null, we fallback to the default
    //   precision found in the culture info (NumberFormatInfo.NumberDecimalDigits).
    //
    // If no specific culture is requested, we use the current culture.
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

            var spec = MoneyFormatSpecifier.Parse(format, money.DecimalPrecision);
            string amount = money.Amount.ToString(spec.AmountFormat, numberFormat ?? NumberFormatInfo.CurrentInfo);
            return FormatImpl(amount, money.Currency.Code, spec);
        }

        public static string FormatMoney(Money money, string format, NumberFormatInfo numberFormat)
        {
            Warrant.NotNull<string>();

            var spec = MoneyFormatSpecifier.Parse(format, money.DecimalPrecision);
            string amount = money.Amount.ToString(spec.AmountFormat, numberFormat ?? NumberFormatInfo.CurrentInfo);
            return FormatImpl(amount, money.Currency.Code, spec);
        }

        public static string FormatPenny(Moneypenny penny, string format, NumberFormatInfo numberFormat)
        {
            Warrant.NotNull<string>();

            // The amount being a long, the decimal precision is simply equal to 0.
            var spec = MoneyFormatSpecifier.Parse(format, 0);
            string amount = penny.Amount.ToString(spec.AmountFormat, numberFormat ?? NumberFormatInfo.CurrentInfo);
            return FormatImpl(amount, penny.PennyOrCurrencyCode, spec);
        }

        private static string FormatImpl(string amount, string currencyCode, MoneyFormatSpecifier spec)
        {
            Warrant.NotNull<string>();

            switch (spec.MainFormat)
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
