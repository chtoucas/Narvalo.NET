// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Globalization;

    using Narvalo.Finance.Properties;

    // Custom formatter for Money:
    // - The amount is formatted using the Currency specifier ("C") for the requested culture.
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
    //   from 0 to 99 and affects the number of digits displayed for the amount.
    //   If no precision is given, we use the decimal precision reported by the object
    //   (the DecimalPrecision property). If DecimalPrecision is null, we fallback to the default
    //   precision found in the culture info (NumberFormatInfo.NumberDecimalDigits).
    //
    // If no specific culture is requested, we use the current culture.
    public sealed class LocalMoneyFormatter : IFormatProvider, ICustomFormatter
    {
        internal const char NumericFormat = 'C';

        #region ICustomFormatter

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            Warrant.NotNull<string>();

            if (arg == null) { return String.Empty; }

            if (arg.GetType() == typeof(Money))
            {
                var money = (Money)arg;

                var spec = MoneyFormatSpecifier.Parse(format, money.DecimalPrecision, NumericFormat);

                return FormatImpl(money.Amount, money.Currency.Code, spec, formatProvider);
            }

            var formattable = arg as IFormattable;
            return formattable == null ? arg.ToString() : formattable.ToString(format, formatProvider);
        }

        #endregion

        #region IFormatProvider

        public object GetFormat(Type formatType)
            => formatType == typeof(LocalMoneyFormatter) ? this : null;

        #endregion

        internal static string FormatImpl(
            decimal amount,
            string currencyCode,
            MoneyFormatSpecifier spec,
            IFormatProvider formatProvider)
        {
            Warrant.NotNull<string>();

            var nfi = (NumberFormatInfo.GetInstance(formatProvider) ?? NumberFormatInfo.CurrentInfo).Copy();

            switch (spec.MainFormat)
            {
                case 'N':
                    // Numeric. Does not include any information about the currency.
                    nfi.CurrencySymbol = String.Empty;
                    nfi.RemoveCurrencySpacing();
                    return amount.ToString(spec.AmountFormat, nfi);
                case 'L':
                    // Left (Currency code placed on the).
                    nfi.CurrencySymbol = String.Empty;
                    nfi.RemoveCurrencySpacing();
                    return currencyCode + " " + amount.ToString(spec.AmountFormat, nfi);
                case 'R':
                    // Right (Currency code placed on the).
                    nfi.CurrencySymbol = String.Empty;
                    nfi.RemoveCurrencySpacing();
                    return amount.ToString(spec.AmountFormat, nfi) + " " + currencyCode;
                case 'G':
                    // General (default). It replaces the currency symbol by the currency code
                    // and ensures that there is a space between the amount and the currency code.
                    nfi.CurrencySymbol = currencyCode;
                    nfi.KeepOrAddCurrencySpacing();
                    return amount.ToString(spec.AmountFormat, nfi);
                default:
                    throw new FormatException(
                        Narvalo.Format.Current(Strings.Money_InvalidFormatSpecification));
            }
        }
    }
}
