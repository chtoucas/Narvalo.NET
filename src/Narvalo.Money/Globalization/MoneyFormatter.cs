// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Globalization
{
    using System;
    using System.Globalization;
    using System.Diagnostics;

    using Narvalo.Properties;

    // FIXME: Update the documentation.
    //
    // Default formatter for all money-like types:
    // - The amount is formatted using the **Number** specifier ("N") for the requested culture.
    // - The position of the currency code depends on the format specifier
    //   (on the left w/ "L"; on the right w/ "R" or "G"; not-included w/ "N").
    // NB: These methods do not handle custom formatters, you must use the method ToString(..)
    // provided by the object.
    //
    // A standard money format string takes the form "Ann" or "AZ", where:
    // - "A" is a single alphabetic character called the format specifier.
    //   Admissible values are "N" or "n" (Numeric), "L" or "l" (Left), "R" or "r" (Right),
    //   and "G" or "g" (General):
    //   * "N" or "n", do not include any information about the currency. Example: 12345.60.
    //   * "L" or "l", place the currency code on the left of the amount. Example: EUR 12345.60.
    //   * "R" or "r", place the currency code on the right of the amount. Example: 12345.60 EUR.
    //   * "G" or "g", same as "R".
    // - "nn" or "Z" is an optional integer called the precision specifier. The precision specifier
    //   "nn" ranges from 0 to 99 and affects the number of digits after the decimal separator to
    //   display; the special value "Z" instructs us to use the number of decimal places defined
    //   by the currency - for those currencies for which the number of decimal places is not known
    //   (legacy currencies for instance), we fallback to the default precision found in the
    //   requested culture info.
    //
    // If a precision specifier is present and the amount has more digits than requested,
    // the **displayed** value is rounded away from zero; it does **not** affect the amount itself.
    //
    // If no precision is given, we use the decimal precision reported by the object
    // (DecimalPlaces for Money and Money<T>; zero for Moneypenny).
    // For Money and Money<T>, if DecimalPlaces is null, we fallback to the default
    // precision found in the culture info (NumberFormatInfo.NumberDecimalDigits).
    //
    // Remark: for a Moneypenny, since the amount is a long, the decimal precision is always equal to 0.
    // - If the currency does not have a minor currency unit, a penny is "really" a money,
    //   eg for the Vietnamese đồng, we display 12345 VND, not 12345 VNd.
    // - Otherwise, we have a "true" penny, eg for the EURO, we display 12345 EUr.
    //
    // Behaviour:
    // - If no format is given, we use the general format ("G").
    // - If no specific culture is requested, we use the default (thread) culture.
    //
    // Custom formatter for Money:
    // - The amount is formatted using the **Currency** specifier ("C") for the requested culture.
    // - The position of the currency code depends on the format specifier
    //   (on the left w/ "L"; on the right w/ "R"; culture-dependent w/ "G"; not-included w/ "N").
    //
    // A standard money format string takes the form "Axx", where:
    // - A is a single alphabetic character called the format specifier.
    //   Admissible values are: "N" (Numeric), "L" (Left), "R" (Right) and "G" (General).
    //   * "N", do not include any information about the currency.
    //   * "L", place the currency code on the left of the amount.
    //   * "R", place the currency code on the right of the amount.
    //   * "G", replaces the currency symbol by the currency code and ensures that there is a space
    //     between the amount and the currency code.
    // - xx is an optional integer called the precision specifier. The precision specifier ranges
    //   from 0 to 99 and affects the number of digits **displayed** for the amount; it does not
    //   round the amount itself. If the precision specifier is present and the amount has more
    //   digits than requested, the displayed value is rounded away from zero.
    //   If no precision is given, we use the decimal precision reported by the object
    //   (the DecimalPlaces property). If DecimalPlaces is null, we fallback to the default
    //   value found in the culture info (NumberFormatInfo.NumberDecimalDigits).
    //
    // Behaviour:
    // - If no format is given, we use the general format ("G").
    // - If no specific culture is requested, we use the current culture.
    //
    // Remarks:
    // - Usually the provider implements both IFormatProvider & ICustomFormatter. I find it very
    //   confusing, so the actual formatter is a separate class.
    //
    // Examples:
    // > money.ToString("R", MoneyFormatInfo.Invariant);
    // or
    // > var provider = new MoneyFormatInfo(new CultureInfo("fr-FR"));
    // > String.Format(provider, "Montant = {0:N}", money);
    internal static class MoneyFormatter
    {
        private const string NO_BREAK_SPACE = "\u00A0";

        public static string FormatMoney<TAmount>(
            MoneyFormat spec,
            TAmount amount,
            string currencyCode,
            NumberFormatInfo nfi)
            where TAmount : IFormattable
        {
            Debug.Assert(nfi != null);

            var format = 'N' + spec.DecimalPlaces?.ToString(CultureInfo.InvariantCulture);

            string value = amount.ToString(format, nfi);

            // Uppercase it (ASCII letter only).
            switch (spec.MainFormat & 0xDF)
            {
                case 'N':
                    // Numeric. Does not include any information about the currency.
                    return value;
                case 'L':
                    // Left (Currency code placed on the).
                    return currencyCode + NO_BREAK_SPACE + value;
                case 'R':
                case 'G':
                    // General (default) or Right (Currency code placed on the).
                    return value + NO_BREAK_SPACE + currencyCode;
                default:
                    throw new FormatException(Format.Current(Strings_Money.Money_InvalidFormatSpecification));
            }
        }
    }
}
