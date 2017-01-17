// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Generic
{
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using Narvalo.Finance.Utilities;

    public class CurrencyUnit<TCurrency> where TCurrency : CurrencyUnit<TCurrency>
    {
        internal CurrencyUnit(short? minorUnits)
        {
            Demand.True(!minorUnits.HasValue || minorUnits >= 0);

            MinorUnits = minorUnits;
        }

        public short? MinorUnits { get; }

        public string Code { get { Warrant.NotNull<string>(); return Name; } }

        public short DecimalPlaces => MinorUnits ?? 0;

        public bool HasFixedDecimalPlaces => DecimalPlaces != Currency.MaxDecimalPlaces;

        public bool IsMetaCurrency => CurrencyHelpers.IsMetaCurrency(Code);

        public bool IsPseudoCurrency => CurrencyHelpers.IsPseudoCurrency(Code, MinorUnits);

        public decimal Epsilon => Currency.Epsilons[DecimalPlaces % Currency.MaxDecimalPlaces];

        private uint Factor => Currency.PowersOfTen[DecimalPlaces % Currency.MaxDecimalPlaces];

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "[Intentionally] When (if?) we add currencies not using a decimal system, this value will no longer look like a constant.")]
        public decimal One => 1m;

        public bool HasMinorCurrency
            => MinorUnits.HasValue
            && MinorUnits.Value != 0
            && MinorUnits.Value != Currency.UnknownMinorUnits;

        protected static string Name
        {
            get { Warrant.NotNull<string>(); return typeof(TCurrency).Name; }
        }

        public Currency ToCurrency() => new Currency(Code, MinorUnits);

        public override string ToString()
        {
            Warrant.NotNull<string>();

            return Code;
        }

        public bool IsNativeTo(CultureInfo cultureInfo)
        {
            Require.NotNull(cultureInfo, nameof(cultureInfo));

            if (cultureInfo.IsNeutralCulture) { return false; }

            var ri = new RegionInfo(cultureInfo.Name);

            return ri.ISOCurrencySymbol == Code;
        }
    }
}
