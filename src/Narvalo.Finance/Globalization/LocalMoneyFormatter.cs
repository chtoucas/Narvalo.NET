// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Globalization;

    using Narvalo.Finance.Properties;

    // Formatting currencies is hard to get right. This formatter uses the culture's rules to
    // format the amount, but to make it work consistently across all .NET cultures, we have to
    // sacrifice the currency symbol.
    // Since it relies entirely on the culture infos, we ignore the number of decimal places
    // specified by the currency.
    public sealed class LocalMoneyFormatter : IFormatProvider, ICustomFormatter
    {
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

            if (format == null || format.Length == 0)
            {
                format = DEFAULT_FORMAT;
            }

            if (numberFormat == null)
            {
                numberFormat = NumberFormatInfo.CurrentInfo;
            }

            switch (format)
            {
                case "N":
                case "n":
                    // Numeric. Does not include any information about the currency.
                    return amount.ToString("C", numberFormat.GetNoSymbolNoSpaceClone());
                case "L":
                case "l":
                    // Left (Currency code placed on the).
                    return currencyCode + " " + amount.ToString("C", numberFormat.GetNoSymbolNoSpaceClone());
                case "R":
                case "r":
                    // Right (Currency code placed on the).
                    return amount.ToString("C", numberFormat.GetNoSymbolNoSpaceClone()) + " " + currencyCode;
                case "G":
                case "g":
                    // General (default). It replaces the currency symbol by the currency code
                    // and ensures that there is a space between the amount and the currency code.
                    return amount.ToString("C", numberFormat.GetCurrencyCodeAndSpaceClone(currencyCode));
                default:
                    throw new FormatException(
                        Narvalo.Format.Current(Strings.Money_InvalidFormatSpecification));
            }
        }
    }
}
