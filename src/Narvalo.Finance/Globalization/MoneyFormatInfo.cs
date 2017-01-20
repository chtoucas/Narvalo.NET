// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Globalization;
    using System.Threading;

    // FIXME: Update the documentation.
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
    //
    // Remarks:
    // - Usually the provider implements both IFormatProvider & ICustomFormatter. I find it very
    //   confusing, so the actual formatter is a separate class.
    //
    // Examples:
    // > money.ToString("R", LocalMoneyFormatter.Current);
    // or
    // > var provider = new LocalMoneyFormatter(new CultureInfo("fr-FR"));
    // > String.Format(provider, "Montant = {0:N}", money);
    public sealed class MoneyFormatInfo : IFormatProvider
    {
        private static readonly Formatter s_Formatter = new Formatter();

        private static MoneyFormatInfo s_InvariantInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoneyFormatInfo"/> class for a specific
        /// <paramref name="provider"/>.
        /// </summary>
        /// <param name="provider">The <see cref="IFormatProvider"/> that will be used to format
        /// the amount. With a composite format, it will also be used to format an
        /// <see cref="IFormattable"/> object which is not of type <see cref="Money"/>.</param>
        public MoneyFormatInfo(IFormatProvider provider)
        {
            Require.NotNull(provider, nameof(provider));
            Provider = provider;
        }

        // Do we use the number of decimal places specified by the currency instead of the
        // decimal precision reported by the object. If you do not use a precision specifier (in the
        // format), we always display the same number of digits for a given currency.
        public bool UseDecimalPlacesFromCurrency { get; set; }

        public bool FormatAmountAsCurrency { get; set; }

        public IFormatProvider Provider { get; }

        public static MoneyFormatInfo CurrentInfo => new MoneyFormatInfo(CultureInfo.CurrentCulture);

        public static MoneyFormatInfo InvariantInfo
        {
            get
            {
                Warrant.NotNull<MoneyFormatInfo>();

                if (s_InvariantInfo == null)
                {
                    var provider = new MoneyFormatInfo(CultureInfo.InvariantCulture);
                    Interlocked.CompareExchange(ref s_InvariantInfo, provider, null);
                }

                return s_InvariantInfo;
            }
        }

        public static MoneyFormatInfo GetInstance(IFormatProvider formatProvider)
        {
            var info = formatProvider as MoneyFormatInfo;
            if (info != null) { return info; }
            if (formatProvider != null)
            {
                return new MoneyFormatInfo(formatProvider);
            }

            return CurrentInfo;
        }

        #region IFormatProvider

        // Let's see how it goes with:
        // > var provider = LocalMoneyFormatProvider.Current;
        //
        // formatType = typeof(Money) is required for Money.ToString(format, IFormatProvider) to work:
        // > money.ToString("N", provider);
        // > formatter = provider.GetFormat(money.GetType()) as ICustomFormatter;
        // where formatter is of type LocalMoneyFormatter,
        // > formatter.Format("N", money, provider);
        // money being of type Money, formatter knows how to format it. NB: it **never** returns null.
        //
        // formatType = typeof(ICustomFormatter) is required for String.Format(IFormatProvider, format, args),
        // and StringBuilder.AppendFormat(IFormatProvider, format, args) to work; see
        // https://github.com/dotnet/coreclr/blob/master/src/mscorlib/src/System/Text/StringBuilder.cs
        // For instance,
        // > String.Format(provider, "{0:N} {1:G}", money, whatever);
        // > formatter = provider.GetFormat(typeof(ICustomFormatter));
        // where formatter is of type LocalMoneyFormatter, then:
        // > formatter.Format("N", money, provider)
        // > formatter.Format("G", whatever, provider)
        // The first call never returns null (see remark above).
        // If the second call returns something, we are done, otherwise, String.Format() checks
        // if 'whatever' implements IFormattable. If it does, it calls:
        // > whatever.ToString("G", provider);
        // otherwise, it falls back to Object.ToString():
        // > whatever.ToString();
        // If (again) the result is null, then it outputs String.Empty.
        // Remark: If we did not handle typeof(ICustomFormatter), String.Format() would check if
        // the object implemented IFormattable. For 'money', since it is the case, it would call
        // money.ToString("N", provider) and we are back to the first case. Everything would work
        // as expected, but it would be less effective.
        //
        // If formatType is anything else, for instance,
        // > whatever.ToString("N", provider);
        // might call
        // > formatter = provider.GetFormat(whatever.GetType());
        // like we do in Money.ToString(). We always return null and it's up to Whatever.ToString()
        // to deal with this. We could return CultureInfo.CurrentCulture.GetFormat(formatType)
        // that is a NumberFormatInfo, a DateTimeFormatInfo or null, matter of taste I think.
        public object GetFormat(Type formatType)
            => formatType == typeof(Money) || formatType == typeof(ICustomFormatter) ? s_Formatter : null;

        #endregion

        private sealed class Formatter : ICustomFormatter
        {
            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                if (arg == null) { return String.Empty; }

                var mfi = formatProvider as MoneyFormatInfo;
                if (mfi == null)
                {
                    // This should never happen. Normally, formatProvider is not null and of type
                    // LocalMoneyFormatter.
                    // Nevertheless, one might call formatter = provider.GetFormat(...) but use the result
                    // with another provider (I don't see why, but we never know):
                    // > formatter.Format(format, arg, anotherProvider);
                    // Rather than trying to deal with it (we could set provider to the current culture)
                    // we prefer to return null since this most certainly means a coding error from the
                    // caller.
                    return null;
                }

                if (arg.GetType() == typeof(Money))
                {
                    return MoneyFormatters.FormatMoney((Money)arg, format, mfi);
                }

                var formattable = arg as IFormattable;
                return formattable == null ? arg.ToString() : formattable.ToString(format, mfi.Provider);
            }
        }
    }
}
