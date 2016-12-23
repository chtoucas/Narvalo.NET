// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Globalization;

    using Narvalo.Finance.Properties;

    internal sealed class MoneyFormatter
    {
        private const string DEFAULT_FORMAT = "G";

        public static string Format(Money money, string format, CultureInfo ci)
        {
            Demand.NotNull(mfi);
            Warrant.NotNull<string>();

            return Format(money.Amount, money.Currency, format, ci);
        }

        public static string Format(decimal amount, Currency currency, string format, CultureInfo ci)
        {
            Demand.NotNull(currency);
            Demand.NotNull(mfi);
            Warrant.NotNull<string>();

            if (format == null || format.Length == 0)
            {
                format = DEFAULT_FORMAT;
            }

            switch (format)
            {
                case "C":
                case "c":
                    // Currency.
                    return FormatCurrency(amount, currency, ci);
                case "A":
                case "a":
                    // Amount.
                    return FormatAmount(amount, currency, ci);
                case "G":
                case "g":
                    // General (default).
                    return FormatGeneral(amount, currency, ci);
                default:
                    throw new FormatException(
                        Narvalo.Format.Current(Strings.Money_InvalidFormatSpecification));
            }
        }

        private static string FormatGeneral(decimal amount, Currency currency, CultureInfo mfi)
        {
            Require.NotNull(mfi, nameof(mfi));
            Demand.NotNull(currency);
            Warrant.NotNull<string>();

            // http://publications.europa.eu/code/en/en-370303.htm
            var nfi = CloneNumberFormat(mfi);

            return currency.Code + "\u00A0" + amount.ToString("C", nfi).Trim();
        }

        private static string FormatAmount(decimal amount, Currency currency, CultureInfo mfi)
        {
            Require.NotNull(mfi, nameof(mfi));
            Demand.NotNull(currency);
            Warrant.NotNull<string>();

            var nfi = CloneNumberFormat(mfi);

            return amount.ToString("C", nfi).Trim();
        }

        private static string FormatCurrency(decimal amount, Currency currency, CultureInfo ci)
        {
            Require.NotNull(ci, nameof(ci));
            Demand.NotNull(currency);
            Warrant.NotNull<string>();

            if (IsCurrencyNative(ci, currency))
            {
                return amount.ToString("C", ci.NumberFormat);
            }
            else
            {
                var nfi = CloneNumberFormat(ci, currency.Code);

                return amount.ToString("C", nfi);
            }
        }

        private static NumberFormatInfo CloneNumberFormat(CultureInfo ci)
            => CloneNumberFormat(ci, String.Empty);

        private static NumberFormatInfo CloneNumberFormat(CultureInfo ci, string currencySymbol)
        {
            var nfi = (NumberFormatInfo)ci.NumberFormat.Clone();
            nfi.CurrencySymbol = currencySymbol;

            return nfi;
        }

        private static bool IsCurrencyNative(CultureInfo ci, Currency currency)
        {
            if (!ci.IsNeutralCulture)
            {
                var ri = new RegionInfo(ci.Name);

                if (ri.ISOCurrencySymbol == currency.Code)
                {
                    return true;
                }
            }

            return false;
        }

        #region MoneyFormatInfo

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
                format = DEFAULT_FORMAT;
            }

            switch (format)
            {
                case "C":
                case "c":
                    // Currency.
                    return FormatCurrency(amount, currency, mfi);
                case "A":
                case "a":
                    // Amount.
                    return FormatAmount(amount, currency, mfi);
                case "G":
                case "g":
                    // General (default).
                    return FormatGeneral(amount, currency, mfi);
                default:
                    throw new FormatException(
                        Narvalo.Format.Current(Strings.Money_InvalidFormatSpecification));
            }
        }

        private static string FormatGeneral(decimal amount, Currency currency, MoneyFormatInfo mfi)
        {
            Require.NotNull(mfi, nameof(mfi));
            Demand.NotNull(currency);
            Warrant.NotNull<string>();

            // http://publications.europa.eu/code/en/en-370303.htm
            var nfi = CloneNumberFormat(mfi);

            return currency.Code + "\u00A0" + amount.ToString("C", nfi).Trim();
        }

        private static string FormatAmount(decimal amount, Currency currency, MoneyFormatInfo mfi)
        {
            Require.NotNull(mfi, nameof(mfi));
            Demand.NotNull(currency);
            Warrant.NotNull<string>();

            var nfi = CloneNumberFormat(mfi);

            return amount.ToString("C", nfi).Trim();
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
                var nfi = CloneNumberFormat(mfi, currency.Code);

                return amount.ToString("C", nfi);
            }
        }

        private static NumberFormatInfo CloneNumberFormat(MoneyFormatInfo mfi)
            => CloneNumberFormat(mfi, String.Empty);

        private static NumberFormatInfo CloneNumberFormat(MoneyFormatInfo mfi, string currencySymbol)
        {
            var nfi = (NumberFormatInfo)mfi.NumberFormat.Clone();
            nfi.CurrencySymbol = currencySymbol;

            return nfi;
        }

        #endregion
    }
}
