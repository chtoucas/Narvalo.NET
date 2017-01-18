// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Globalization;

    // Similar to MoneyFormatter, but use the number of decimal places specified by the currency
    // instead of the decimal precision reported by the object. It is "fixed" in the sense that,
    // if you do not use a precision specifier, we always display the same number of digits for
    // a given currency.
    public sealed class FixedMoneyFormatter : IFormatProvider, ICustomFormatter
    {
        internal const char NumericFormat = 'C';

        private readonly IFormatProvider _provider;

        public FixedMoneyFormatter() : this(CultureInfo.CurrentCulture) { }

        public FixedMoneyFormatter(IFormatProvider provider)
        {
            _provider = provider;
        }

        public static FixedMoneyFormatter Current => new FixedMoneyFormatter(CultureInfo.CurrentCulture);

        public static FixedMoneyFormatter Invariant => new FixedMoneyFormatter(CultureInfo.InvariantCulture);

        #region ICustomFormatter

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            Warrant.NotNull<string>();

            if (arg == null) { return String.Empty; }

            if (arg.GetType() == typeof(Money))
            {
                var money = (Money)arg;

                var spec = MoneyFormatSpecifier.Parse(format, money.Currency.DecimalPlaces);
                string amount = money.Amount.ToString(spec.AmountFormat, _provider);
                return MoneyFormatters.FormatImpl(amount, money.Currency.Code, spec);
            }

            var formattable = arg as IFormattable;
            return formattable == null ? arg.ToString() : formattable.ToString(format, formatProvider);
        }

        #endregion

        #region IFormatProvider

        public object GetFormat(Type formatType)
            => formatType == typeof(Money) || formatType == typeof(ICustomFormatter) ? this : null;

        #endregion
    }
}
