// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Globalization;

    // See:
    // - Formatting Types in the .NET Framework
    //   https://msdn.microsoft.com/en-us/library/26etazsy(v=vs.110).aspx
    // - How to: Define and Use Custom Numeric Format Providers
    //  https://msdn.microsoft.com/en-us/library/bb762932(v=vs.110).aspx
    // - Composite Formatting
    //  https://msdn.microsoft.com/en-us/library/txafckwd(v=vs.110).aspx
    // - Standard Numeric Format Strings
    //  https://msdn.microsoft.com/en-us/library/dwhawy9k(v=vs.110).aspx
    // http://stackoverflow.com/questions/36005332/custom-currency-format-in-c-sharp
    public sealed class MoneyFormatter : IFormatProvider, ICustomFormatter
    {
        private static readonly MoneyFormatter s_Formatter = new MoneyFormatter();

        #region IFormatProvider implementation.

        // Method called by the framework before an object's implementation of IFormattable.ToString(),
        // whenever we use String.Format() or StringBuilder.AppendFormat().
        public object GetFormat(Type formatType)
            => formatType == typeof(ICustomFormatter) ? s_Formatter : null;
            // REVIEW: CultureInfo.CurrentCulture.GetFormat(formatType) instead of null?

        #endregion

        #region ICustomFormatter implementation.

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg == null) { return String.Empty; }

            if (arg.GetType() == typeof(Money) && format == "C")
            {
                throw new NotImplementedException();
            }
            if (arg.GetType() == typeof(Money<>) && format == "C")
            {
                throw new NotImplementedException();
            }

            var formattable = arg as IFormattable;
            return formattable == null
                ? arg.ToString()
                : formattable.ToString(format, formatProvider);
        }

        #endregion

        public static string Format(decimal amount, Currency currency, CultureInfo cultureInfo)
        {
            var ri = new RegionInfo(cultureInfo.Name);

            if (ri.ISOCurrencySymbol == currency.Code)
            {
                return amount.ToString("0:C", cultureInfo);
            }
            else
            {
                var nfi = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
                nfi.CurrencySymbol = CurrencySymbol.GetFallbackSymbol(currency.Code);

                return amount.ToString("0:C", nfi);
            }
        }

        public static string Format(Money money, CultureInfo cultureInfo)
        {
            var ri = new RegionInfo(cultureInfo.Name);

            if (ri.ISOCurrencySymbol == money.Currency.Code)
            {
                return money.Amount.ToString("0:C", cultureInfo);
            }
            else
            {
                var nfi = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
                nfi.CurrencySymbol = CurrencySymbol.GetFallbackSymbol(money.Currency.Code);

                return money.Amount.ToString("0:C", nfi);
            }
        }

        public static string Format<TCurrency>(Money<TCurrency> money, CultureInfo cultureInfo)
            where TCurrency : Currency
        {
            throw new NotImplementedException();
        }
    }
}
