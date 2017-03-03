// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Generic
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using Narvalo.Internal;

    public class Currency<TCurrency> : Internal.ICurrencyUnit
        where TCurrency : Currency<TCurrency>
    {
        internal Currency(short? minorUnits)
        {
            Demand.True(!minorUnits.HasValue || minorUnits >= 0);

            MinorUnits = minorUnits;
        }

        public short? MinorUnits { get; }

        public string Code => Name;

        public int DecimalPlaces => MinorUnits ?? 0;

        public bool HasFixedDecimalPlaces => DecimalPlaces != Currency.MaxDecimalPlaces;

        public int? FixedDecimalPlaces => HasFixedDecimalPlaces ? DecimalPlaces : (int?)null;

        public bool IsMetaCurrency => CurrencyHelpers.IsMetaCurrency(Code);

        public bool IsPseudoCurrency => CurrencyHelpers.IsPseudoCurrency(Code, MinorUnits);

        public decimal Epsilon => CurrencyHelpers.Epsilons[DecimalPlaces % Currency.MaxDecimalPlaces];

        [CLSCompliant(false)]
        public uint Factor => CurrencyHelpers.PowersOfTen[DecimalPlaces % Currency.MaxDecimalPlaces];

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "[Intentionally] When (if?) we add currencies not using a decimal system, this value will no longer look like a constant.")]
        public decimal One => 1m;

        protected static string Name => typeof(TCurrency).Name;

        public Currency ToCurrency() => new Currency(Code, MinorUnits);

        public override string ToString() => Code;

        public bool IsNativeTo(CultureInfo cultureInfo) => CurrencyHelpers.IsNativeTo(Code, cultureInfo);

        internal decimal ConvertToMajor(decimal minor) => Epsilon * minor;

        internal decimal ConvertToMinor(decimal major) => Factor * major;
    }
}
