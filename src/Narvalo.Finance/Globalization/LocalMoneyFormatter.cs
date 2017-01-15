// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Globalization;

    using Narvalo.Finance.Properties;

    public sealed class LocalMoneyFormatter : IFormatProvider, ICustomFormatter
    {
        private const string AMOUNT_FORMAT = "C";
        private const string DEFAULT_FORMAT = "G";

        #region ICustomFormatter

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            Warrant.NotNull<string>();

            if (arg == null) { return String.Empty; }

            if (arg.GetType() == typeof(Money))
            {
                var money = (Money)arg;

                return FormatImpl(
                    money.Amount,
                    money.Currency.Code,
                    format,
                    NumberFormatInfo.GetInstance(formatProvider));
            }

            var formattable = arg as IFormattable;
            return formattable == null
                ? arg.ToString()
                : formattable.ToString(format, formatProvider);
        }

        #endregion

        #region IFormatProvider

        public object GetFormat(Type formatType)
            => formatType == typeof(LocalMoneyFormatter) ? this : null;

        #endregion

        internal static string FormatImpl(
            decimal amount,
            string currencyCode,
            string format,
            NumberFormatInfo numberFormat)
        {
            Warrant.NotNull<string>();

            if (format == null || format.Length == 0) { format = DEFAULT_FORMAT; }
            if (format.Length != 1) { throw new FormatException("XXX"); }

            var nfi = (numberFormat ?? NumberFormatInfo.CurrentInfo).Copy();

            // Take the first char and uppercase it (ASCII only).
            switch (format[0] & 0xDF)
            {
                case 'N':
                    // Numeric. Does not include any information about the currency.
                    nfi.CurrencySymbol = String.Empty;
                    nfi.RemoveCurrencySpacing();
                    return amount.ToString(AMOUNT_FORMAT, nfi);
                case 'L':
                    // Left (Currency code placed on the).
                    nfi.CurrencySymbol = String.Empty;
                    nfi.RemoveCurrencySpacing();
                    return currencyCode + " " + amount.ToString(AMOUNT_FORMAT, nfi);
                case 'R':
                    // Right (Currency code placed on the).
                    nfi.CurrencySymbol = String.Empty;
                    nfi.RemoveCurrencySpacing();
                    return amount.ToString(AMOUNT_FORMAT, nfi) + " " + currencyCode;
                case 'G':
                    // General (default). It replaces the currency symbol by the currency code
                    // and ensures that there is a space between the amount and the currency code.
                    nfi.CurrencySymbol = currencyCode;
                    nfi.KeepOrAddCurrencySpacing();
                    return amount.ToString(AMOUNT_FORMAT, nfi);
                default:
                    throw new FormatException(
                        Narvalo.Format.Current(Strings.Money_InvalidFormatSpecification));
            }
        }
    }
}
