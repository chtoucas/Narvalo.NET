// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Globalization;

    using Narvalo.Finance.Properties;

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
    //   (the DecimalPrecision property). If DecimalPrecision is null, we fallback to the default
    //   precision found in the culture info (NumberFormatInfo.NumberDecimalDigits).
    //
    // Behaviour:
    // - If no format is given, we use the general format ("G").
    // - If no specific culture is requested, we use the current culture.
    public sealed class LocalMoneyFormatter : IFormatProvider, ICustomFormatter
    {
        internal const char NumericFormat = 'C';

        // Provider for formatting the amount; if null, the current culture is used.
        private readonly IFormatProvider _provider;

        public LocalMoneyFormatter() : this(CultureInfo.CurrentCulture) { }

        public LocalMoneyFormatter(IFormatProvider provider)
        {
            _provider = provider;
        }

        public static LocalMoneyFormatter Current => new LocalMoneyFormatter(CultureInfo.CurrentCulture);

        public static LocalMoneyFormatter Invariant => new LocalMoneyFormatter(CultureInfo.InvariantCulture);

        #region ICustomFormatter

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            Warrant.NotNull<string>();

            // REVIEW: We could check that it is called from himself? See below.
            //if (!Equals(formatProvider)) { return String.Empty; }

            if (arg == null) { return String.Empty; }

            if (arg.GetType() == typeof(Money))
            {
                var money = (Money)arg;

                var spec = MoneyFormatSpecifier.Parse(format, money.DecimalPrecision, NumericFormat);

                return FormatImpl(money.Amount, money.Currency.Code, spec, _provider);
            }

            var formattable = arg as IFormattable;
            return formattable == null ? arg.ToString() : formattable.ToString(format, formatProvider);
        }

        #endregion

        #region IFormatProvider

        // Two possibilities with:
        // > var provider = new LocalMoneyFormatter();
        //
        // typeof(Money) is needed for Money.ToString(format, IFormatProvider):
        // > money.ToString("N", provider);
        // it calls provider.GetFormat(money.GetType()), then
        // > provider.Format("N", money, provider)
        //
        // typeof(ICustomFormatter) is needed for String.Format(IFormatProvider, format, args),
        // and StringBuilder.AppendFormat(IFormatProvider, format, args):
        // > String.Format(provider, "{0:N}", money);
        // String.Format() calls provider.GetFormat(typeof(ICustomFormatter)), then
        // > provider.Format("N", money, provider)
        //
        // If we did not handle typeof(ICustomFormatter), since money implements IFormattable,
        // String.Format() would call money.ToString("N", null) and the provider would be ignored.
        // See https://msdn.microsoft.com/en-us/library/bb762932(v=vs.110).aspx.
        public object GetFormat(Type formatType)
            => formatType == typeof(Money) || formatType == typeof(ICustomFormatter) ? this : null;

        #endregion

        internal static string FormatImpl(
            decimal amount,
            string currencyCode,
            MoneyFormatSpecifier spec,
            IFormatProvider provider)
        {
            Demand.NotNull(provider);
            Warrant.NotNull<string>();

            var nfi = NumberFormatInfo.GetInstance(provider).Copy();

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
