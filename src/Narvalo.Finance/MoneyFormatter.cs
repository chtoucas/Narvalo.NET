// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Globalization;

    // See https://msdn.microsoft.com/fr-fr/library/dwhawy9k(v=vs.110).aspx
    // and https://msdn.microsoft.com/en-us/library/txafckwd(v=vs.110).aspx
    public sealed class MoneyFormatter : IFormatProvider
    {
        private readonly MoneyFormatter_ _formatter = new MoneyFormatter_();

        public object GetFormat(Type formatType)
            => formatType == typeof(ICustomFormatter)
            ? _formatter
            : CultureInfo.CurrentCulture.GetFormat(formatType);

        private sealed class MoneyFormatter_ : ICustomFormatter
        {
            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                if (arg == null) { return String.Empty; }

                if (arg.GetType() == typeof(Decimal) && format == "C")
                {
                    //return Format((Money)arg, formatProvider);
                    throw new NotImplementedException();
                }
                //if (arg.GetType() == typeof(Money<>) && format == "C")
                //{
                //    throw new NotImplementedException();
                //}

                var formattable = arg as IFormattable;
                return formattable == null
                    ? arg.ToString()
                    : formattable.ToString(format, formatProvider);
            }

            private string Format(Money money, CultureInfo cultureInfo)
            {
                var ri = new RegionInfo(cultureInfo.Name);

                if (ri.ISOCurrencySymbol == money.Currency.Code)
                {
                    return money.Amount.ToString("0:C", cultureInfo);
                }
                else
                {
                    var numberFormat = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
                    numberFormat.CurrencySymbol = "XXX";

                    return money.Amount.ToString("0:C", numberFormat);
                }
            }
        }
    }
}
