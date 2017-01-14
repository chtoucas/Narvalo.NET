// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Globalization;

    using Narvalo.Finance.Generic;

    public sealed class LocalMoneyFormatter<TCurrency>
        : IFormatProvider, ICustomFormatter
        where TCurrency : CurrencyUnit<TCurrency>
    {
        #region ICustomFormatter

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            Warrant.NotNull<string>();

            if (arg == null) { return String.Empty; }

            if (arg.GetType() == typeof(Money<TCurrency>))
            {
                var money = (Money<TCurrency>)arg;

                LocalMoneyFormatter.FormatImpl(
                    money.Amount,
                    money.Unit.Code,
                    format,
                    NumberFormatInfo.GetInstance(formatProvider));
            }

            var formattable = arg as IFormattable;
            return formattable == null
                ? arg.ToString()
                : formattable.ToString(format, formatProvider);
        }

        #endregion

        #region IFormatProvider

        public object GetFormat(Type formatType)
            => formatType == typeof(LocalMoneyFormatter<TCurrency>) ? this : null;

        #endregion
    }
}
