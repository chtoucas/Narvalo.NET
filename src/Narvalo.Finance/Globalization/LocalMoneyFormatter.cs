// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Globalization;

    using Narvalo.Finance.Properties;

    public sealed class LocalMoneyFormatter : IFormatProvider, ICustomFormatter
    {
        #region ICustomFormatter

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            Warrant.NotNull<string>();

            if (arg == null) { return String.Empty; }

            if (arg.GetType() == typeof(Money))
            {
                var money = (Money)arg;

                var fmt = FormatParts.Parse(format, money.DecimalPrecision);

                return FormatImpl(
                    money.Amount,
                    money.Currency.Code,
                    fmt,
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
            FormatParts format,
            NumberFormatInfo numberFormat)
        {
            Warrant.NotNull<string>();

            format.DefaultAmountFormat = "C";

            var nfi = (numberFormat ?? NumberFormatInfo.CurrentInfo).Copy();

            switch (format.MainFormat)
            {
                case 'N':
                    // Numeric. Does not include any information about the currency.
                    nfi.CurrencySymbol = String.Empty;
                    nfi.RemoveCurrencySpacing();
                    return amount.ToString(format.AmountFormat, nfi);
                case 'L':
                    // Left (Currency code placed on the).
                    nfi.CurrencySymbol = String.Empty;
                    nfi.RemoveCurrencySpacing();
                    return currencyCode + " " + amount.ToString(format.AmountFormat, nfi);
                case 'R':
                    // Right (Currency code placed on the).
                    nfi.CurrencySymbol = String.Empty;
                    nfi.RemoveCurrencySpacing();
                    return amount.ToString(format.AmountFormat, nfi) + " " + currencyCode;
                case 'G':
                    // General (default). It replaces the currency symbol by the currency code
                    // and ensures that there is a space between the amount and the currency code.
                    nfi.CurrencySymbol = currencyCode;
                    nfi.KeepOrAddCurrencySpacing();
                    return amount.ToString(format.AmountFormat, nfi);
                default:
                    throw new FormatException(
                        Narvalo.Format.Current(Strings.Money_InvalidFormatSpecification));
            }
        }
    }
}
