// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Globalization
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
            bool isFund)
        {
            return new CurrencyInfo(code, numericCode) {
                EnglishName = englishName,
                EnglishRegionName = englishRegionName,
                IsFund = isFund,
                MinorUnits = minorUnits,
            };
        }

        protected static CurrencyInfo CreateLegacyCurrency(
            string code,
            short numericCode,
            string englishName,
            string englishRegionName,
            bool isFund)
        {
            return new CurrencyInfo(code, numericCode) {
                EnglishName = englishName,
                EnglishRegionName = englishRegionName,
                Superseded = true,
                IsFund = isFund,
            };
        }
    }
}
