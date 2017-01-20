// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Threading;

    using Narvalo.Finance.Generic;

    // Custom formatter for Money<T>. For explanations, please see MoneyFormatInfo.
    // TODO: Merge this and MoneyFormatInfo.
    public sealed class MoneyFormatInfo<TCurrency> : IFormatProvider
        where TCurrency : Currency<TCurrency>
    {
        private static readonly Formatter s_Formatter = new Formatter();

        private static MoneyFormatInfo<TCurrency> s_Invariant;

        public MoneyFormatInfo(IFormatProvider provider)
        {
            Require.NotNull(provider, nameof(provider));
            Provider = provider;
        }

        public bool UseDecimalPlacesFromCurrency { get; set; }

        public bool FormatAmountAsCurrency { get; set; }

        public IFormatProvider Provider { get; }

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static MoneyFormatInfo<TCurrency> CurrentInfo
            => new MoneyFormatInfo<TCurrency>(CultureInfo.CurrentCulture);

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static MoneyFormatInfo<TCurrency> InvariantInfo
        {
            get
            {
                Warrant.NotNull<MoneyFormatInfo<TCurrency>>();

                if (s_Invariant == null)
                {
                    var provider = new MoneyFormatInfo<TCurrency>(CultureInfo.InvariantCulture);
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

                var mfi = formatProvider as MoneyFormatInfo<TCurrency>;
                if (mfi == null) { return null; }

                if (arg.GetType() == typeof(Money<TCurrency>))
                {
                    return MoneyFormatters.FormatMoney((Money<TCurrency>)arg, format, mfi);
                }

                var formattable = arg as IFormattable;
                return formattable == null ? arg.ToString() : formattable.ToString(format, mfi.Provider);
            }
        }
    }
}
