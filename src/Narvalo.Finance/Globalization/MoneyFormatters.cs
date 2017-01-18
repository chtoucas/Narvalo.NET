// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Globalization;

    using Narvalo.Finance.Generic;
    using Narvalo.Finance.Properties;

    // Default formatter for all money-like types:
    // - The amount is formatted using the **Number** specifier ("N") for the requested culture.
    // - The position of the currency code depends on the format specifier
    //   (on the left w/ "L"; on the right w/ "R" or "G"; not-included w/ "N").
    //
    // A standard money format string takes the form "Axx", where:
    // - A is a single alphabetic character called the format specifier.
    //   Admissible values are "N" (Numeric), "L" (Left), "R" (Right) and "G" (General):
    //   * "N", do not include any information about the currency. Example: 12345.60.
    //   * "L", place the currency code on the left of the amount. Example: EUR 12345.60.
    //   * "R" and "G", place the currency code on the right of the amount. Example: 12345.60 EUR.
    // - xx is an optional integer called the precision specifier. The precision specifier ranges
    //   from 0 to 99 and affects the number of digits **displayed** for the amount; it does not
    //   round the amount itself. If the precision specifier is present and the amount has more
    //   digits than requested, the displayed value is rounded away from zero.
    //   If no precision is given, we use the decimal precision reported by the object
    //   (DecimalPrecision for Money and Money<T>; for Moneypenny, see below).
    //   For Money and Money<T>, if DecimalPrecision is null, we fallback to the default
    //   precision found in the culture info (NumberFormatInfo.NumberDecimalDigits).
    //
    // Remark: for a Moneypenny, since the amount is a long, the decimal precision is always equal to 0.
    // - If the currency does not have a minor currency unit, a penny is "really" a money,
    //   eg for the Vietnamese đồng, we display 12345 VND, not 12345 VNd.
    // - Otherwise, we have a "true" penny, eg for the EURO, we display 12345 EUr.
    //
    // Behaviour:
    // - If no format is given, we use the general format ("G").
    // - If no specific culture is requested, we use the default culture.
    public static class MoneyFormatters
    {
        private const string NO_BREAK_SPACE = "\u00A0";

        public static string FormatMoney<TCurrency>(Money<TCurrency> money, string format, IFormatProvider provider)
            where TCurrency : Currency<TCurrency>
        {
            Warrant.NotNull<string>();

            var spec = MoneyFormatSpecifier.Parse(format, money.DecimalPrecision);
            string amount = money.Amount.ToString(spec.AmountFormat, provider);
            return FormatImpl(amount, money.Currency.Code, spec);
        }

        public static string FormatMoney(Money money, string format, IFormatProvider provider)
        {
            Warrant.NotNull<string>();

            var spec = MoneyFormatSpecifier.Parse(format, money.DecimalPrecision);
            string amount = money.Amount.ToString(spec.AmountFormat, provider);
            return FormatImpl(amount, money.Currency.Code, spec);
        }

        public static string FormatPenny(Moneypenny penny, string format, IFormatProvider provider)
        {
            Warrant.NotNull<string>();

            var spec = MoneyFormatSpecifier.Parse(format, 0);
            string amount = penny.Amount.ToString(spec.AmountFormat, provider);
            return FormatImpl(amount, penny.PennyOrCurrencyCode, spec);
        }

        internal static string FormatImpl(string amount, string currencyCode, MoneyFormatSpecifier spec)
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
