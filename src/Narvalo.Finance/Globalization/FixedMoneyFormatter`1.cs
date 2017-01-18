// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Globalization;

    using Narvalo.Finance.Generic;

    // Custom formatter for Money<T>. For explanations, please see FixedMoneyFormatter.
    public sealed class FixedMoneyFormatter<TCurrency>
        : IFormatProvider, ICustomFormatter
        where TCurrency : Currency<TCurrency>
    {
        #region ICustomFormatter

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            Warrant.NotNull<string>();

            if (arg == null) { return String.Empty; }

            if (arg.GetType() == typeof(Money<TCurrency>))
            {
                var money = (Money<TCurrency>)arg;

                var spec = MoneyFormatSpecifier.Parse(format, money.Currency.DecimalPlaces);
                string amount = money.Amount.ToString(spec.AmountFormat, formatProvider);
                return MoneyFormatter.FormatImpl(amount, money.Currency.Code, spec);
            }

            var formattable = arg as IFormattable;
            return formattable == null ? arg.ToString() : formattable.ToString(format, formatProvider);
        }

        #endregion

        #region IFormatProvider

        public object GetFormat(Type formatType)
            => formatType == typeof(FixedMoneyFormatter<TCurrency>) ? this : null;

        #endregion
    }
}
