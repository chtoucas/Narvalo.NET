// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Globalization;

    using Narvalo.Finance.Generic;
    using Narvalo.Finance.Properties;

    public sealed class LocalMoneyFormatter : IFormatProvider, ICustomFormatter
    {
        private const string DEFAULT_FORMAT = "G";

        #region ICustomFormatter

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg == null) { return String.Empty; }

            if (arg.GetType() == typeof(Money))
            {
                return Format((Money)arg, format, formatProvider);
            }
            if (arg.GetType() == typeof(Money<>))
            {
                throw new NotImplementedException();
            }

            var formattable = arg as IFormattable;
            return formattable == null
                ? arg.ToString()
                : formattable.ToString(format, formatProvider);
        }

        #endregion

        #region IFormatProvider

        // Method called by the framework before an object's implementation of IFormattable.ToString(),
        // whenever we use String.Format() or StringBuilder.AppendFormat().
        public object GetFormat(Type formatType)
            => formatType == typeof(LocalMoneyFormatter) ? this : null;

        #endregion

        public static string Format(Money money, string format, IFormatProvider formatProvider)
        {
            Warrant.NotNull<string>();
            return Format(money, format, NumberFormatInfo.GetInstance(formatProvider));
        }

        public static string Format(Money money, string format, NumberFormatInfo nfi)
        {
            Warrant.NotNull<string>();
            return Format(money.Amount, money.Currency.Code, format, nfi);
        }

        private static string Format(decimal amount, string currencyCode, string format, NumberFormatInfo nfi)
        {
            Warrant.NotNull<string>();

            if (nfi == null)
            {
                nfi = NumberFormatInfo.CurrentInfo;
            }

            if (format == null || format.Length == 0)
            {
                format = DEFAULT_FORMAT;
            }

            switch (format)
            {
                case "N":
                case "n":
                    // Numeric. Does not include any information about the currency.
                    return amount.ToString("C", nfi.GetNoSymbolNoSpaceClone());
                case "L":
                case "l":
                    // Left (Currency code placed on the).
                    return currencyCode + "\u00A0" + amount.ToString("C", nfi.GetNoSymbolNoSpaceClone());
                case "R":
                case "r":
                    // Right (Currency code placed on the).
                    return amount.ToString("C", nfi.GetNoSymbolNoSpaceClone()) + "\u00A0" + currencyCode;
                case "G":
                case "g":
                    // General (default). It replaces the currency symbol by the currency code
                    // and ensures that there is a space between the amount and the currency code.
                    return amount.ToString("C", nfi.GetCurrencyCodeAndSpaceClone(currencyCode));
                default:
                    throw new FormatException(
                        Narvalo.Format.Current(Strings.Money_InvalidFormatSpecification));
            }
        }
    }
}
