﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Generic
{
    using System.Globalization;

    public class CurrencyUnit<TCurrency> where TCurrency : CurrencyUnit<TCurrency>
    {
        private readonly int? _minorUnits;

        internal CurrencyUnit(int? minorUnits) { _minorUnits = minorUnits; }

        protected static string Name
        {
            get { Warrant.NotNull<string>(); return typeof(TCurrency).Name; }
        }

        public Currency ToCurrency() => new Currency(Code, MinorUnits);

        public string Code
        {
            get { Warrant.NotNull<string>(); return Name; }
        }

        public int? MinorUnits => _minorUnits;

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
