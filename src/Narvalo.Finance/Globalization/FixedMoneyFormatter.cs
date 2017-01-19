// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Globalization;
    using System.Threading;

    // Similar to MoneyFormatters.FormatMoney, but use the number of decimal places specified
    // by the currency instead of the decimal precision reported by the object. It is "fixed"
    // in the sense that, if you do not use a precision specifier, we always display the same
    // number of digits for a given currency.
    public sealed class FixedMoneyFormatter : IFormatProvider
    {
        private static readonly Formatter s_Formatter = new Formatter();

        private static FixedMoneyFormatter s_Invariant;

        public FixedMoneyFormatter(IFormatProvider provider)
        {
            Provider = provider;
        }

        public IFormatProvider Provider { get; }

        public static FixedMoneyFormatter Current => new FixedMoneyFormatter(CultureInfo.CurrentCulture);

        public static FixedMoneyFormatter Invariant
        {
            get
            {
                Warrant.NotNull<FixedMoneyFormatter>();

                if (s_Invariant == null)
                {
                    var provider = new FixedMoneyFormatter(CultureInfo.InvariantCulture);
                    Interlocked.CompareExchange(ref s_Invariant, provider, null);
                }

                return s_Invariant;
            }
        }

        #region IFormatProvider

        public object GetFormat(Type formatType)
            => formatType == typeof(Money) || formatType == typeof(ICustomFormatter) ? s_Formatter : null;

        #endregion

        private sealed class Formatter : ICustomFormatter
        {
            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                if (arg == null) { return String.Empty; }

                IFormatProvider provider = (formatProvider as FixedMoneyFormatter)?.Provider;
                if (provider == null) { return null; }

                if (arg.GetType() == typeof(Money))
                {
                    var money = (Money)arg;

                    var spec = MoneyFormatSpecifier.Parse(format, money.Currency.DecimalPlaces);
                    string amount = money.Amount.ToString(spec.AmountFormat, provider);
                    return MoneyFormatters.DefaultFormat(amount, money.Currency.Code, spec);
                }

                var formattable = arg as IFormattable;
                return formattable == null ? arg.ToString() : formattable.ToString(format, provider);
            }
        }
    }
}
