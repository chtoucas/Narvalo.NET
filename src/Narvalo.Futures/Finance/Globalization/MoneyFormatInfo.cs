// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Globalization;
    using System.Threading;

    public sealed class MoneyFormatInfo : IFormatProvider
    {
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
        //public bool UseDecimalPlacesFromCurrency { get; set; }

        // The format that will be used to format the amount part.
        //public bool UseCurrencyLayoutFromProvider { get; set; }

        //public string CurrencyAmountSeparator { get; set; } = NO_BREAK_SPACE;

        public IFormatProvider Provider { get; }

        public static MoneyFormatInfo CurrentInfo => GetInstance(CultureInfo.CurrentCulture);

        public static MoneyFormatInfo InvariantInfo
        {
            get
            {
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

            //var ci = provider as CultureInfo;
            //if (ci != null) { return new MoneyFormatInfo(ci); }

            if (formatProvider != null)
            {
                info = formatProvider.GetFormat(typeof(MoneyFormatInfo)) as MoneyFormatInfo;

                return info ?? new MoneyFormatInfo(formatProvider);
            }

            return CurrentInfo;
        }

        #region IFormatProvider

        public object GetFormat(Type formatType) => formatType == typeof(MoneyFormatInfo) ? this : null;

        #endregion
    }
}
