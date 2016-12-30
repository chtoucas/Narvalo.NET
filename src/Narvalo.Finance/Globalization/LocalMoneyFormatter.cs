// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;

    using Narvalo.Finance.Generic;

    public sealed class LocalMoneyFormatter : IFormatProvider, ICustomFormatter
    {
        #region ICustomFormatter

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg == null) { return String.Empty; }

            if (arg.GetType() == typeof(Money) && format == "L")
            {
                throw new NotImplementedException();
            }
            if (arg.GetType() == typeof(Money<>) && format == "L")
            {
                throw new NotImplementedException();
            }

            var formattable = arg as IFormattable;
            return formattable == null
                ? arg.ToString()
                : formattable.ToString(format, formatProvider);
        }

        #endregion

        #region IFormatProvider

        // Method called by the framework before an object's implementation of IFormattable.ToString(),
        // whenever we use String.Format() or StringBuilder.AppendFormat().
        public object GetFormat(Type formatType)
            => formatType == typeof(LocalMoneyFormatter) ? this : null;

        #endregion
    }
}
