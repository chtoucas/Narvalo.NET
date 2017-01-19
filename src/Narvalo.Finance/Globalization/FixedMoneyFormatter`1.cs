// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Threading;

    using Narvalo.Finance.Generic;

    // Custom formatter for Money<T>. For explanations, please see FixedMoneyFormatter.
    public sealed class FixedMoneyFormatter<TCurrency> : IFormatProvider
        where TCurrency : Currency<TCurrency>
    {
        private static readonly Formatter s_Formatter = new Formatter();

        private static FixedMoneyFormatter<TCurrency> s_Invariant;

        public FixedMoneyFormatter(IFormatProvider provider)
        {
            Provider = provider;
        }

        public IFormatProvider Provider { get; }

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static FixedMoneyFormatter<TCurrency> Current
            => new FixedMoneyFormatter<TCurrency>(CultureInfo.CurrentCulture);

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static FixedMoneyFormatter<TCurrency> Invariant
        {
            get
            {
                Warrant.NotNull<FixedMoneyFormatter<TCurrency>>();

                if (s_Invariant == null)
                {
                    var provider = new FixedMoneyFormatter<TCurrency>(CultureInfo.InvariantCulture);
                    Interlocked.CompareExchange(ref s_Invariant, provider, null);
                }

                return s_Invariant;
            }
        }

        #region IFormatProvider

        public object GetFormat(Type formatType)
            => formatType == typeof(Money<TCurrency>) || formatType == typeof(ICustomFormatter) ? s_Formatter : null;

        #endregion

        private sealed class Formatter : ICustomFormatter
        {
            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                if (arg == null) { return String.Empty; }

                IFormatProvider provider = (formatProvider as FixedMoneyFormatter)?.Provider;
                if (provider == null) { return null; }

                if (arg.GetType() == typeof(Money<TCurrency>))
                {
                    var money = (Money<TCurrency>)arg;

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
