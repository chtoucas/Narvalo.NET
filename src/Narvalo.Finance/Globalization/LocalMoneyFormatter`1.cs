// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Globalization;

    using Narvalo.Finance.Generic;

    // Custom formatter for Money<T>. For explanations, please see LocalMoneyFormatter.
    public sealed class LocalMoneyFormatter<TCurrency> : IFormatProvider, ICustomFormatter
        where TCurrency : Currency<TCurrency>
    {
        private readonly IFormatProvider _provider;

        public LocalMoneyFormatter() { }

        public LocalMoneyFormatter(IFormatProvider provider)
        {
            _provider = provider;
        }

        public static LocalMoneyFormatter<TCurrency> Current
            => new LocalMoneyFormatter<TCurrency>(CultureInfo.CurrentCulture);

        public static LocalMoneyFormatter<TCurrency> Invariant
            => new LocalMoneyFormatter<TCurrency>(CultureInfo.InvariantCulture);

        #region ICustomFormatter

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            Warrant.NotNull<string>();

            if (arg == null) { return String.Empty; }

            if (arg.GetType() == typeof(Money<TCurrency>))
            {
                var money = (Money<TCurrency>)arg;

                var spec = MoneyFormatSpecifier.Parse(
                    format, money.DecimalPrecision, LocalMoneyFormatter.NumericFormat);

                LocalMoneyFormatter.FormatImpl(money.Amount, money.Currency.Code, spec, _provider);
            }

            var formattable = arg as IFormattable;
            return formattable == null ? arg.ToString() : formattable.ToString(format, formatProvider);
        }

        #endregion

        #region IFormatProvider

        public object GetFormat(Type formatType)
            => formatType == typeof(Money<TCurrency>) || formatType == typeof(ICustomFormatter) ? this : null;

        #endregion
    }
}
