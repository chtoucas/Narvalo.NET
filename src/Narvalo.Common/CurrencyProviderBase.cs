// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System.Collections.Generic;

    public abstract class CurrencyProviderBase : ICurrencyProvider
    {
        public abstract HashSet<string> CurrencyCodes { get; }

        public abstract IEnumerable<CurrencyInfo> GetCurrencies(CurrencyTypes types);

        public virtual string GetFallbackSymbol(string code)
        {
            return "\x00a4";
        }

        protected static CurrencyInfo CreateCurrency(
            string code,
            short numericCode,
            short? minorUnits,
            string englishName,
            string englishRegionName,
            bool isFund = false)
        {
            return new CurrencyInfo(code, numericCode) {
                EnglishName = englishName,
                EnglishRegionName = englishRegionName,
                IsDiscontinued = false,
                IsFund = isFund,
                MinorUnits = minorUnits,
            };
        }

        protected static CurrencyInfo CreateLegacyCurrency(
            string code,
            short numericCode,
            short? minorUnits,
            string englishName,
            string englishRegionName,
            bool isFund = false)
        {
            return new CurrencyInfo(code, numericCode) {
                EnglishName = englishName,
                EnglishRegionName = englishRegionName,
                IsDiscontinued = true,
                IsFund = isFund,
                MinorUnits = minorUnits,
            };
        }
    }
}
