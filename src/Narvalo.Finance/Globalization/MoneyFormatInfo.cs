// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Globalization;

    using Narvalo.Finance.Properties;

    [Obsolete]
    internal sealed class MoneyFormatInfo : IFormatProvider
    {
        private readonly Currency _currency;
        private readonly NumberFormatInfo _numberFormat;

        private MoneyFormatInfo(NumberFormatInfo numberFormat, Currency currency)
        {
            Require.NotNull(numberFormat, nameof(numberFormat));
            Require.NotNull(currency, nameof(currency));

            _numberFormat = numberFormat;
            _currency = currency;
        }

        public NumberFormatInfo NumberFormat
        {
            get { Warrant.NotNull<NumberFormatInfo>(); return _numberFormat; }
        }

        public Currency Currency { get { Warrant.NotNull<Currency>(); return _currency; } }

        public bool CurrencyIsNative { get; private set; }

        public static MoneyFormatInfo Create(CultureInfo cultureInfo, Currency currency)
        {
            Require.NotNull(cultureInfo, nameof(cultureInfo));
            Require.NotNull(currency, nameof(currency));

            if (currency.IsNative(cultureInfo))
            {
                return new MoneyFormatInfo(cultureInfo.NumberFormat, currency) { CurrencyIsNative = true, };
            }

            return new MoneyFormatInfo(cultureInfo.NumberFormat, currency);
        }

        #region IFormatProvider implementation.

        // Method called by the framework before an object's implementation of IFormattable.ToString(),
        // whenever we use String.Format() or StringBuilder.AppendFormat().
        public object GetFormat(Type formatType)
            => formatType == typeof(MoneyFormatInfo) ? this : null;

        #endregion

        public static MoneyFormatInfo GetCurrentInfo(Currency currency)
        {
            Expect.NotNull(currency);

            return Create(CultureInfo.CurrentCulture, currency);
        }

        public static MoneyFormatInfo GetInstance(IFormatProvider provider, Currency currency)
        {
            Expect.NotNull(currency);

            var mfi = provider as MoneyFormatInfo;
            if (mfi != null) { return mfi; }

            var ci = provider as CultureInfo;
            if (ci != null) { return Create(ci, currency); }

            if (provider != null)
            {
                mfi = provider.GetFormat(typeof(MoneyFormatInfo)) as MoneyFormatInfo;
                if (mfi != null) { return mfi; }
            }

            return Create(CultureInfo.CurrentCulture, currency);
        }

        #region Formatting

        public static string Format(Money money, string format, MoneyFormatInfo mfi)
        {
            Demand.NotNull(mfi);
            Warrant.NotNull<string>();

            return Format(money.Amount, money.Currency, format, mfi);
        }

        public static string Format(decimal amount, Currency currency, string format, MoneyFormatInfo mfi)
        {
            Demand.NotNull(currency);
            Demand.NotNull(mfi);
            Warrant.NotNull<string>();

            if (format == null || format.Length == 0)
            {
                format = "G";
            }

            switch (format)
            {
                case "C":
                case "c":
                    // Currency.
                    return FormatCurrency(amount, currency, mfi);
                case "N":
                case "n":
                    // No currency symbol.
                    return amount.ToString("C", mfi.NumberFormat.GetNoSymbolNoSpaceClone());
                case "G":
                case "g":
                    // General (default).
                    // http://publications.europa.eu/code/en/en-370303.htm
                    // .Replace(' ', '\u00A0')
                    return currency.Code + "\u00A0"
                        + amount.ToString("C", mfi.NumberFormat.GetNoSymbolNoSpaceClone());
                default:
                    throw new FormatException(
                        Narvalo.Format.Current(Strings.Money_InvalidFormatSpecification));
            }
        }

        private static string FormatCurrency(decimal amount, Currency currency, MoneyFormatInfo mfi)
        {
            Require.NotNull(mfi, nameof(mfi));
            Demand.NotNull(currency);
            Warrant.NotNull<string>();

            if (mfi.CurrencyIsNative)
            {
                return amount.ToString("C", mfi.NumberFormat);
            }
            else
            {
                return amount.ToString("C", mfi.NumberFormat.GetCurrencyCodeAndSpaceClone(currency));
            }
        }

        #endregion
    }
}
